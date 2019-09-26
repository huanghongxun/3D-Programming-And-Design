using UnityEngine;
using System.Linq;

public class GameController : SceneController
{

    private Coast westCoast, eastCoast;
    private Boat boat;
    private Game game;

    void Awake()
    {
        game = gameObject.AddComponent<Game>();
        game.GameStateChanged += delegate
        {
            if (game.state == GameState.Win)
            {
                gameObject.AddComponent<DisableEntityAction>();
                gameObject.GetComponent<GuiIngame>().state = game.state;
            }
            else if (game.state == GameState.Lose)
            {
                gameObject.AddComponent<DisableEntityAction>();
                gameObject.GetComponent<GuiIngame>().state = game.state;
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
