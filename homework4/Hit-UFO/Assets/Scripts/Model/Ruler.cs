using UnityEngine;
using System.Collections;

public class Ruler
{
    private readonly int round;
    private readonly Game game;
    private float time = 0;

    public int Trial { get; private set; }

    public int MaxTrial { get; private set; } = 10;

    public int Score { get; private set; }

    public Ruler(Game game, int round)
    {
        this.game = game;
        this.round = round;
        this.Trial = this.MaxTrial = round;
        this.time = 10f / MaxTrial;
        this.Score = 0;
    }

    public void SendUFO()
    {
        var ufo = new UFOModel
        {
            score = Random.Range(1, 5),
            game = this
        };
        UFO ufoEntity = UFO.Factory.Instance.Instantiate(ufo);
        ufoEntity.gameObject.transform.parent = game.transform;

        float speed = 0.1f;
        for (int i = 1; i < round; ++i) speed *= 1.1f;

        float actualSpeed = Random.Range(speed, speed * 1.3f);
        Rigidbody body = ufo.Send(ufoEntity.renderer.initialPosition, ufoEntity.renderer.initialDirection, actualSpeed);
        // body.AddTorque(new Vector3(1, 0, 0) * 20);
    }

    public void Update()
    {
        time += Time.deltaTime;
        while (time >= 10f / MaxTrial && Trial > 0)
        {
            Trial--;
            time -= 10f / MaxTrial;
            SendUFO();
        }

        foreach (var ufo in game.GetComponentsInChildren<UFO>())
        {
            if (!ufo.model.success && ufo.transform.position.y < 0)
                EntityRendererFactory.Instance.Collect(ufo.gameObject);
        }

        if (Trial <= 0 && game.GetComponentsInChildren<UFO>().Length == 0)
        {
            if (Score < round)
            {
                Debug.Log(this.GetHashCode());
                game.RoundLose();
            }
            else
            {
                game.RoundWin();
            }
        }
    }

    public void AddScore(int score)
    {
        Debug.Log(this.GetHashCode());
        this.Score += score;
    }
}
