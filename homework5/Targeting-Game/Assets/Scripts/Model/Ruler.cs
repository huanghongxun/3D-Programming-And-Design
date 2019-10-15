using UnityEngine;
using System.Collections;

public class Ruler
{
    private readonly int round;
    private readonly Game game;
    private float time = 0;
    private bool shooting = true;
    private bool aiming = false;
    private bool shot = false;
    private Arrow heldArrow;

    public int Trial { get; private set; }

    public int MaxTrial { get; private set; } = 10;

    public int Score { get; private set; }

    public float Strength { get; private set; }

    public Vector2 Wind { get; set; }

    public Ruler(Game game, int round)
    {
        this.game = game;
        this.round = round;
        this.Trial = this.MaxTrial = round;
        this.time = 10f / MaxTrial;
        this.Score = 0;
    }

    private float Time2Strength(float time, float duration, float maxStrength)
    {
        while (time > 2 * duration) time -= 2 * duration;
        if (time < duration) return maxStrength * time / duration;
        else return maxStrength * (2 * duration - time) / duration;
    }

    public void Update()
    {
        if (shooting)
        {
            if (!heldArrow)
            {
                Wind = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));

                heldArrow = Arrow.Factory.Instance.Instantiate(new ArrowModel
                {
                    game = this
                });
                heldArrow.transform.position = new Vector3(0, 3, -8);
            }

            if (!shot)
                heldArrow.AimAt(Camera.main.ScreenPointToRay(Input.mousePosition).direction);

            if (Input.GetMouseButtonDown(0))
            {
                aiming = true;
                time = 0;
                Strength = 0;
            }
            if (Input.GetMouseButtonUp(0))
            {
                shot = true;
                aiming = false;
                heldArrow.Shoot(Camera.main.ScreenPointToRay(Input.mousePosition).direction * Strength * 5);
                heldArrow.gameObject.GetComponent<Rigidbody>().AddForce(Wind.x, Wind.y, 0);
            }
            else if (aiming)
            {
                // progress
                time += Time.deltaTime;
                Strength = Time2Strength(time, 2, 1);
            }
        }
    }

    private float[] scoreDist = new float[] { 3.85f, 3.76f, 3.663f, 3.57f, 3.473f, 3.38f, 3.283f, 3.195f, 3.1f, 3.005f, 2.9f};

    public int AddScore(Vector3 point)
    {
        float dist = (new Vector3(point.x, point.y, 0) - new Vector3(0, 2.9f, 0)).magnitude + 2.9f;
        int score = 0;
        for (int i = 0; i < scoreDist.Length; ++i)
            if (dist > scoreDist[i])
            {
                score = i;
                break;
            }

        this.Score += score;

        TipAction action = TipAction.StartTipAction();
        action.Duration = 2f;
        action.Color = Color.green;
        action.Text = "+" + score;
        action.Position = new Vector2(Screen.width / 2f, Screen.height / 2f);

        return score;
    }

    public void NextTrial()
    {
        heldArrow = null;
        shooting = true;
        shot = false;
        Trial++;
    }
}
