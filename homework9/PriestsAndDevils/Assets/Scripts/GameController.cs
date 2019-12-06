using System;
using UnityEngine;
using System.Linq;

public class GameController : SceneController
{

    private Coast westCoast, eastCoast;
    private Boat boat;
    private Game game;
    private GuiIngame gui;

    void Awake()
    {
        game = gameObject.AddComponent<Game>();
        gui = gameObject.AddComponent<GuiIngame>();
        game.GameStateChanged += delegate
        {
            if (game.state == BoatState.Win)
            {
                gameObject.AddComponent<DisableEntityAction>();
                gui.state = game.state;
            }
            else if (game.state == BoatState.Lose)
            {
                gameObject.AddComponent<DisableEntityAction>();
                gui.state = game.state;
            }
        };
        gui.Next += delegate
        {
            GameStates.GameAction action = game.NextAction();
            if (action != null)
            {
                if (game.state == BoatState.East)
                {
                    foreach (GameObject obj in boat.model.GetOnBoat())
                    {
                        if (obj != null) boat.model.GoOnShore(obj);
                    }

                    for (var i = 0; i < -action.devilDifference; ++i)
                    {
                        eastCoast.model.GoOffShore(eastCoast.model.GetOnShore()
                            .Where(obj => obj != null && obj.GetComponent<Devil>() != null)
                            .DefaultIfEmpty(null).FirstOrDefault());
                    }

                    for (var i = 0; i < -action.priestDifference; ++i)
                    {
                        eastCoast.model.GoOffShore(eastCoast.model.GetOnShore()
                            .Where(obj => obj != null && obj.GetComponent<Priest>() != null)
                            .DefaultIfEmpty(null).FirstOrDefault());
                    }
                }
                else
                {
                    foreach (GameObject obj in boat.model.GetOnBoat())
                    {
                        if (obj != null) boat.model.GoOnShore(obj);
                    }

                    for (var i = 0; i < action.devilDifference; ++i)
                    {
                        westCoast.model.GoOffShore(westCoast.model.GetOnShore()
                            .Where(obj => obj != null && obj.GetComponent<Devil>() != null)
                            .DefaultIfEmpty(null).FirstOrDefault());
                    }

                    for (var i = 0; i < action.priestDifference; ++i)
                    {
                        westCoast.model.GoOffShore(westCoast.model.GetOnShore()
                            .Where(obj => obj != null && obj.GetComponent<Priest>() != null)
                            .DefaultIfEmpty(null).FirstOrDefault());
                    }
                }
            }
            else
            {
                gui.ShowWarning("Dead game");
            }
        };

        SSDirector.GetInstance().OnSceneWake(this);
    }

    public override void LoadResources()
    {
        DisableEntityAction action = gameObject.GetComponent<DisableEntityAction>();
        if (action) Destroy(action);

        GameObject water = Instantiate("River", "Prefabs/River");
        water.transform.parent = transform;
        water.transform.position = new Vector3(0, 1, 0);
        water.transform.localScale = new Vector3(200, 1, 200);

        GameObject terrain = Instantiate("River", "Prefabs/Terrain");
        terrain.transform.parent = transform;

        westCoast = InstantiateByFactory<CoastRenderer.CoastRendererFactory, CoastRenderer, Coast, CoastModel>("WestCoast");
        westCoast.transform.parent = transform;
        westCoast.transform.position = new Vector3(-7, 1, 0);
        westCoast.transform.Rotate(Vector3.up, 180);
        westCoast.model.BoatStopPosition = new Vector3(-4, 1.25f, 0);
        westCoast.model.BoatStopDirection = new Vector3(0, 0, -1);
        game.westCoast = westCoast.model;

        eastCoast = InstantiateByFactory<CoastRenderer.CoastRendererFactory, CoastRenderer, Coast, CoastModel>("EastCoast");
        eastCoast.transform.parent = transform;
        eastCoast.transform.position = new Vector3(7, 1, 0);
        eastCoast.model.BoatStopPosition = new Vector3(4, 1.25f, 0);
        eastCoast.model.BoatStopDirection = new Vector3(0, 0, 1);
        game.eastCoast = eastCoast.model;

        boat = InstantiateByFactory<BoatRenderer.BoatRendererFactory, BoatRenderer, Boat, BoatModel>("Boat");
        boat.model.StopAt(eastCoast.model, ActionStyle.Immediately);
        boat.Action += (sender, args) =>
        {
            if (boat.model.GetOnBoat().Count(c => c != null) == 0) return;

            CoastModel coast = boat.transform.parent.GetComponent<CoastModel>();
            if (coast == westCoast.model)
            {
                boat.model.StopAt(eastCoast.model);
            }
            else if (coast == eastCoast.model)
            {
                boat.model.StopAt(westCoast.model);
            }
        };
        game.boat = boat.model;

        for (var i = 0; i < 3; ++i)
        {
            var priest = Instantiate<Priest>("Priest" + i, "Prefabs/Priest");
            eastCoast.model.GoOnShore(priest.gameObject, ActionStyle.Immediately);
        }

        for (var i = 0; i < 3; ++i)
        {
            var devil = Instantiate<Devil>("Devil" + i, "Prefabs/Devil");
            eastCoast.model.GoOnShore(devil.gameObject, ActionStyle.Immediately);
        }

        game.StartGame();
    }

    public override void Restart()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        LoadResources();
    }

}
