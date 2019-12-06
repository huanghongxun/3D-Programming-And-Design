using System;
using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    public GameStates states = new GameStates();
    public GameStates.GameState gameState;
    public BoatState state;
    public CoastModel westCoast, eastCoast;
    public BoatModel boat;

    public event EventHandler GameStateChanged;
    
    public void StartGame()
    {
        gameState = states.GetInitialState();
        boat.StoppedAt += (sender, args) =>
        {
            if (args.coast == eastCoast)
                state = BoatState.East;
            else if (args.coast == westCoast)
                state = BoatState.West;
            CheckGameState();
        };
        state = BoatState.East;
    }

    private void CheckGameState()
    {
        if (state != BoatState.West && state != BoatState.East) return;

        var west = westCoast.GetOnShore().Where(c => c != null);
        var westPriests = west.Count(c => c.GetComponent<Priest>());
        var westDevils = west.Count(c => c.GetComponent<Devil>());
        var east = eastCoast.GetOnShore().Where(c => c != null);
        var eastPriests = east.Count(c => c.GetComponent<Priest>());
        var eastDevils = east.Count(c => c.GetComponent<Devil>());
        var boat = this.boat.GetOnBoat().Where(c => c != null);
        var boatPriests = boat.Count(c => c.GetComponent<Priest>());
        var boatDevils = boat.Count(c => c.GetComponent<Devil>());

        if (state == BoatState.East)
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
            state = BoatState.Win;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (westPriests < westDevils && westPriests > 0 ||
            eastPriests < eastDevils && eastPriests > 0)
        {
            state = BoatState.Lose;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }
        
        gameState = new GameStates.GameState(state, westPriests, westDevils, eastPriests, eastDevils);
    }

    public GameStates.GameAction NextAction()
    {
        return states.GetState(gameState)?.nextWinAction;
    }
}
