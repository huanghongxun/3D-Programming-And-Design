using UnityEngine;

/// <summary>
/// 笑脸按钮，点击将重新开始游戏
/// 按钮状态依赖游戏棋盘状态
/// </summary>
public class GuiStateButton : GuiComponent
{
    private GuiTileBoard board;
    private StateButtonScript images;

    public GuiStateButton(GuiTileBoard board, StateButtonScript images)
    {
        this.board = board;
        this.images = images;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        Sprite image = null;
        switch (board.state)
        {
            case GuiTileBoard.GameState.Lose:
                image = images.LoseSprite;
                break;
            case GuiTileBoard.GameState.Uninitialized:
                goto case GuiTileBoard.GameState.Playing;
            case GuiTileBoard.GameState.Playing:
                if (mousePressed[0])
                    image = images.PressedSprite;
                else
                    image = images.DefaultSprite;
                break;
            case GuiTileBoard.GameState.Win:
                image = images.WinSprite;
                break;
        }
        SpriteDrawUtility.DrawSprite(image, new Rect(GetPosition(), size), GUI.color);
    }

}
