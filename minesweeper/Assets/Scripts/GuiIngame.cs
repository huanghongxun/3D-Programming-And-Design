using UnityEngine;

/// <summary>
/// 保存方格的材质
/// </summary>
public class GuiIngame : BaseBehaviour
{
    public int GridWidth, GridHeight, TotalMines;

    private GuiTileBoard board;
    private GuiDigitPanel panel;
    private int mines;

    private void Start()
    {
        screen = new GuiScreen();

        board = new GuiTileBoard(new Vector2Int(GridWidth, GridHeight))
        {
            images = GetComponent<TileScript>(),
            position = () => new Vector2(Screen.width / 2, Screen.height / 2),
            size = new Vector2(GridWidth * 30, GridHeight * 30),
            alignment = Alignment.Center,
            totalMines = TotalMines
        };

        screen.AddChild(board);
        GuiStateButton button = new GuiStateButton(board, GetComponent<StateButtonScript>())
        {
            position = () => new Vector2(Screen.width / 2, 20),
            size = new Vector2(30, 30),
            alignment = Alignment.TopCenter
        };
        screen.AddChild(button);
        button.MouseClicked += delegate
        {
            Start();
        };

        panel = new GuiDigitPanel(GetComponent<DigitScript>(), 3)
        {
            position = () => new Vector2(Screen.width / 2 + 100, 20)
        };
        screen.AddChild(panel);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        panel.number = board.bombsLeft;
    }

    private void OnDestroy()
    {

    }
}
