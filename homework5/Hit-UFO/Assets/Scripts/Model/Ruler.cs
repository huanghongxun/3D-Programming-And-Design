using UnityEngine;
using System.Collections;

public class Ruler
{
    private readonly int round;
    private readonly Game game;
    private float time = 0;
    private readonly IActionManager acm;

    public int Trial { get; private set; }

    public int MaxTrial { get; private set; } = 10;

    public int Score { get; private set; }

    public Ruler(Game game, IActionManager acm, int round)
    {
        this.game = game;
        this.acm = acm;
        this.round = round;
        this.Trial = this.MaxTrial = round;
        this.time = 10f / MaxTrial;
        this.Score = 0;
    }

    public void SendUFO()
    {
        acm.SendUFO(game, this, round);
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
            if (!ufo.model.success && acm.IsUFODead(ufo))
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
