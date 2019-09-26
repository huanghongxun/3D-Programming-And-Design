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
        boat.StoppedAt += (sender, args) =>
        {
            if (args.coast == eastCoast)
                state = GameState.East;
            else if (args.coast == westCoast)
                state = GameState.West;
            CheckGameState();
        };
        state = GameState.East;
    }

    private void CheckGameState()
    {
        if (state != GameState.West && state != GameState.East) return;

        var west = westCoast.GetOnShore().Where(c => c != null);
        var westPriests = west.Count(c => c.GetComponent<Priest>());
        var westDevils = west.Count(c => c.GetComponent<Devil>());
        var east = eastCoast.GetOnShore().Where(c => c != null);
        var eastPriests = east.Count(c => c.GetComponent<Priest>());
        var eastDevils = east.Count(c => c.GetComponent<Devil>());
        var boat = this.boat.GetOnBoat().Where(c => c != null);
        var boatPriests = boat.Count(c => c.GetComponent<Priest>());
        var boatDevils = boat.Count(c => c.GetComponent<Devil>());

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

        if (westPriests + westDevils == 6)
        {
            state = GameState.Win;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (westPriests < westDevils && westPriests > 0 ||
            eastPriests < eastDevils && eastPriests > 0)
        {
            state = GameState.Lose;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
