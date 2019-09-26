using System;
using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    public GameState state;
    public CoastModel westCoast, eastCoast;
    public BoatModel boat;

    public event EventHandler GameStateChanged;

    public void StartGame()
    {
        boat.StoppedAt += delegate
        {
            if (state == GameState.West)
                state = GameState.East;
            else if (state == GameState.East)
                state = GameState.West;
            CheckGameState();
        };
        state = GameState.East;
    }

    private void CheckGameState()
    {
        if (state != GameState.West && state != GameState.East) return;

        var west = westCoast.GetOnShore().Where(c => c != null);
        var westPriests = west.Count(c => c is Priest);
        var westDevils = west.Count(c => c is Devil);
        var east = eastCoast.GetOnShore().Where(c => c != null);
        var eastPriests = east.Count(c => c is Priest);
        var eastDevils = east.Count(c => c is Devil);
        var boat = this.boat.GetOnBoat().Where(c => c != null);
        var boatPriests = boat.Count(c => c is Priest);
        var boatDevils = boat.Count(c => c is Devil);

        if (west.Count() == 6)
        {
            state = GameState.Win;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (state == GameState.East)
        {
            eastPriests += boatPriests;
            eastDevils += boatDevils;
        }
        else
        {
            westPriests += boatPriests;
            westDevils += boatDevils;
        }

        if (westPriests < westDevils && westPriests > 0 ||
            eastPriests < eastDevils && eastPriests > 0)
        {
            state = GameState.Lose;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
