using UnityEngine;
using System.Collections;

public class GameController : SceneController
{
    public Texture StrengthTexture;
    private Game game;
    private GuiIngame view;

    public override void Awake()
    {
        game = gameObject.AddComponent<Game>();
        view = gameObject.AddComponent<GuiIngame>();
        view.strengthTexture = StrengthTexture;

        base.Awake();
    }

    public override void LoadResources()
    {
        // 本游戏没有要初始化的资源
    }

    public override void Restart()
    {
        game.Round = 1;
    }

    void Update()
    {
        view.state = game.State;
        view.trial = game.Ruler.Trial;
        view.score = game.Ruler.Score;
        view.strength = game.Ruler.Strength;
        view.wind = game.Ruler.Wind;
    }
}
