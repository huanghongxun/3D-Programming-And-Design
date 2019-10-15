using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public GameState State = GameState.Running;

    private int round;

    public int Round
    {
        get { return round; }
        set
        {
            round = value;
            ResetGame();
        }
    }

    public int MaxRound { get; set; } = 10;

    public Ruler Ruler { get; set; }

    private float time;

    public void ResetGame()
    {
        // 第 i 轮发出 i 个飞碟
        Ruler = new Ruler(this, round);
        State = GameState.Running;
    }

    void Start()
    {
        Round = 1;
    }

    void Update()
    {
        if (State != GameState.Running) return;

        Ruler?.Update();
    }

    public void RoundLose()
    {
        State = GameState.Lose;
    }

    public void RoundWin()
    {
        if (Round >= MaxRound)
        {
            State = GameState.Win;
        }
        else
        {
            Round++;
        }
    }
}
