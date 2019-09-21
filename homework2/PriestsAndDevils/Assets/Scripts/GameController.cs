using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : SceneController
{

    private GameState state;
    private Coast westCoast, eastCoast;
    private Boat boat;

    void Awake()
    {
        SSDirector.GetInstance().OnSceneWake(this);


    }

    public override void LoadResources()
    {
        state = GameState.East;
        DisableEntityAction action = gameObject.GetComponent<DisableEntityAction>();
        if (action) Destroy(action);

        GameObject river = Instantiate("River", "Prefabs/River");
        river.transform.parent = transform;
        river.transform.position = new Vector3(0, 0.5f, 0);

        westCoast = Instantiate<Coast>("WestCoast", "Prefabs/Coast");
        westCoast.transform.parent = transform;
        westCoast.transform.position = new Vector3(-7, 1, 0);
        westCoast.BoatStopPosition = new Vector3(-4, 1.25f, 0);

        eastCoast = Instantiate<Coast>("EastCoast", "Prefabs/Coast");
        eastCoast.transform.parent = transform;
        eastCoast.transform.position = new Vector3(7, 1, 0);
        eastCoast.BoatStopPosition = new Vector3(4, 1.25f, 0);

        boat = Instantiate<Boat>("Boat", "Prefabs/Boat");
        boat.StopAt(eastCoast);
        boat.Action += (sender, args) =>
        {
            if (boat.GetOnBoat().Count(c => c != null) == 0) return;

            Coast coast = boat.transform.parent.GetComponent<Coast>();
            if (coast == westCoast)
            {
                state = GameState.East;
                boat.StopAt(eastCoast);
            }
            else if (coast == eastCoast)
            {
                state = GameState.West;
                boat.StopAt(westCoast);
            }

            CheckGameState();
        };

        for (var i = 0; i < 3; ++i)
        {
            var priest = Instantiate<Priest>("Priest" + i, "Prefabs/Priest");
            priest.Action += delegate { CheckGameState(); };
            eastCoast.GoOnShore(priest);
        }

        for (var i = 0; i < 3; ++i)
        {
            var devil = Instantiate<Devil>("Devil" + i, "Prefabs/Devil");
            devil.Action += delegate { CheckGameState(); };
            eastCoast.GoOnShore(devil);
        }
    }

    public override void Restart()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        LoadResources();
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
            gameObject.AddComponent<DisableEntityAction>();
            gameObject.GetComponent<GuiIngame>().state = state;
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
            gameObject.AddComponent<DisableEntityAction>();
            gameObject.GetComponent<GuiIngame>().state = state;
            return;
        }
    }
}
