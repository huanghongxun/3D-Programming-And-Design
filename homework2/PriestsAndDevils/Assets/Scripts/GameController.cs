using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : SceneController
{
    public enum GameState
    {
        West,
        East,
        Win,
        Lose
    }

    private GameState state = GameState.East;
    private Coast westCoast, eastCoast;
    private Boat boat;
    private IList<Character> characters = new List<Character>();

    void Awake()
    {
        SSDirector.GetInstance().OnSceneWake(this);


    }

    public override void LoadResources()
    {
        GameObject river = Instantiate("River", "Prefabs/River");
        river.transform.position = new Vector3(0, 0.5f, 0);

        westCoast = Instantiate<Coast>("WestCoast", "Prefabs/Coast");
        westCoast.transform.position = new Vector3(7, 1, 0);
        westCoast.BoatStopPosition = new Vector3(4, 1.25f, 0);

        eastCoast = Instantiate<Coast>("EastCoast", "Prefabs/Coast");
        eastCoast.transform.position = new Vector3(-7, 1, 0);
        eastCoast.BoatStopPosition = new Vector3(-4, 1.25f, 0);

        boat = Instantiate<Boat>("Boat", "Prefabs/Boat");
        boat.transform.position = eastCoast.BoatStopPosition;

        for (var i = 0; i < 3; ++i)
        {
            var priest = Instantiate<Priest>("Priest" + i, "Prefabs/Priest");
            eastCoast.GoOnShore(priest);
        }

        for (var i = 0; i < 3; ++i)
        {
            var devil = Instantiate<Devil>("Devil" + i, "Prefabs/Devil");
            eastCoast.GoOnShore(devil);
        }
    }

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void Resume()
    {
        throw new System.NotImplementedException();
    }

    private void CheckGameState()
    {
        if (state != GameState.West && state != GameState.East) return;

        var west = westCoast.GetOnShore();
        var westPriests = west.Count(c => c is Priest);
        var westDevils = west.Count(c => c is Devil);
        var east = eastCoast.GetOnShore();
        var eastPriests = east.Count(c => c is Priest);
        var eastDevils = east.Count(c => c is Devil);
        var boat = this.boat.GetOnBoat();
        var boatPriests = boat.Count(c => c is Priest);
        var boatDevils = boat.Count(c => c is Devil);

        if (west.Count() == 6)
        {
            state = GameState.Win;
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
        }
    }
}
