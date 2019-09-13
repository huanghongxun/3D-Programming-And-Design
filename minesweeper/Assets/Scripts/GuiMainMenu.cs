using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 主界面的控制器
/// </summary>
public class GuiMainMenu : BaseBehaviour
{
    public GUIStyle labelGuiStyle, buttonGuiStyle;
    public Sprite normal, pressed, over;

    private void Start()
    {
        screen = new GuiScreen();

        GuiLabel label = new GuiLabel
        {
            position = () => new Vector2(Screen.width / 2, Screen.height / 2 - 60),
            style = labelGuiStyle,
            content = new GuiContent("Minesweeper"),
            size = new Vector2(300, 50),
            alignment = Alignment.BottomCenter
        };
        screen.AddChild(label);

        GuiSpriteButton startButton = new GuiSpriteButton(normal, over, pressed)
        {
            position = () => new Vector2(Screen.width / 2, Screen.height / 2 - 5),
            style = buttonGuiStyle,
            content = new GuiContent("开始游戏"),
            size = new Vector2(100, 40),
            alignment = Alignment.BottomCenter
        };
        screen.AddChild(startButton);

        startButton.MouseClicked += delegate
        {
            SceneManager.LoadScene("IngameScene");
        };

        GuiSpriteButton quitButton = new GuiSpriteButton(normal, over, pressed)
        {
            position = () => new Vector2(Screen.width / 2, Screen.height / 2 + 5),
            style = buttonGuiStyle,
            content = new GuiContent("退出"),
            size = new Vector2(100, 40),
            alignment = Alignment.TopCenter
        };
        screen.AddChild(quitButton);

        quitButton.MouseClicked += delegate
        {
            Application.Quit();
        };
    }

}
