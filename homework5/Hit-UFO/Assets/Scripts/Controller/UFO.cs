using UnityEngine;

public class UFO : EntityRenderee<UFOModel, UFORenderer>
{
    private static readonly Color[] colors = new Color[] { Color.white, Color.red, Color.blue, Color.black, Color.yellow, Color.gray };

    public override void OnAction(GameObject gameObject)
    {
        model.OnShot();

        TipAction action = TipAction.StartTipAction();
        action.Color = colors[model.score];
        action.Duration = 3;
        var pos = Input.mousePosition;
        pos.y = Screen.height - pos.y;
        action.Position = pos;
        action.Text = "+" + model.score;
    }

    void Update()
    {
        renderer.SetColor(colors[model.score]);
    }

    public void Send(float speed)
    {
        model.Send(renderer.initialPosition, renderer.initialDirection, speed);
    }

    public class Factory : EntityFactory<UFO, UFOModel, UFORenderer, Factory>
    {
        public override UFORenderer InstantiateImpl(UFOModel model)
        {
            if (model.score <= 2)
                return EntityRendererFactory.Instance.CreateGameObject<UFORenderer>("Prefabs/Sphere");
            else
                return EntityRendererFactory.Instance.CreateGameObject<UFORenderer>("Prefabs/UFO");
        }
    }
}
