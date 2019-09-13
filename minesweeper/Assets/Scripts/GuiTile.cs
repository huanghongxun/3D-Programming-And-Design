using UnityEngine;
using System;

/// <summary>
/// GuiTile 表示一个格子，这个格子可能是地雷。
/// 该类将保存该格状态。
/// </summary>
public class GuiTile : GuiComponent
{
    public enum TileState
    {
        Normal,
        Flagged,
        Question,
        Revealed,
        Dead,
        Mine,
        WrongMine
    }

    private Sprite image;
    private GuiTileBoard board;
    private Vector2Int pos;

    public TileState state { get; set; }

    public GuiTile(GuiTileBoard board, Vector2Int pos)
    {
        this.board = board;
        this.pos = pos;

        MouseClicked += (sender, args) =>
        {
            if (!board.isPlaying) return;

            if (args.code == KeyCode.Mouse0 && state == TileState.Normal)
            {
                board.Reveal(pos);
            }

            if (args.code == KeyCode.Mouse1)
            {
                if (state == TileState.Flagged)
                    state = TileState.Question;
                else if (state == TileState.Question)
                    state = TileState.Normal;
                else if (state == TileState.Normal)
                    state = TileState.Flagged;
            }

            if (state == TileState.Revealed && args.code == KeyCode.Mouse2)
            {
                board.RevealAround(pos);
            }
        };
    }

    /// <summary>
    /// 判断当前格子的周围 8 个格子是否存在鼠标中键点击的格子
    /// 如果存在，表示当前格子为中键点击的格子的相邻格子，需要被一起按下。
    /// </summary>
    private bool isMiddlePressed()
    {
        for (int i = Math.Max(0, pos.x - 1); i <= pos.x + 1 && i < board.gridSize.x; ++i)
            for (int j = Math.Max(0, pos.y - 1); j <= pos.y + 1 && j < board.gridSize.y; ++j)
                if (board.tiles[i, j].mousePressed[2] && board.tiles[i, j].state == TileState.Revealed)
                    return true;
        return false;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        switch (state)
        {
            case TileState.Normal:
                if (board.isPlaying && (mousePressed[0] || mousePressed[1] || mousePressed[2]))
                    image = board.images.TilePressed;
                else if (board.isPlaying && isMiddlePressed())
                    image = board.images.Tile0;
                else
                    image = board.images.TileUnknown;
                break;
            case TileState.Flagged:
                image = board.images.TileFlag;
                break;
            case TileState.Question:
                if (board.isPlaying && (mousePressed[0] || mousePressed[1] || mousePressed[2]))
                    image = board.images.TileQuestionPressed;
                else
                    image = board.images.TileQuestion;
                break;
            case TileState.Revealed:
                switch (board.GetMines(pos))
                {
                    case 0: image = board.images.Tile0; break;
                    case 1: image = board.images.Tile1; break;
                    case 2: image = board.images.Tile2; break;
                    case 3: image = board.images.Tile3; break;
                    case 4: image = board.images.Tile4; break;
                    case 5: image = board.images.Tile5; break;
                    case 6: image = board.images.Tile6; break;
                    case 7: image = board.images.Tile7; break;
                    case 8: image = board.images.Tile8; break;
                }
                break;
            case TileState.Dead:
                image = board.images.TileDead;
                break;
            case TileState.Mine:
                image = board.images.TileMine;
                break;
            case TileState.WrongMine:
                image = board.images.TileWrong;
                break;
        }

        SpriteDrawUtility.DrawSprite(image, new Rect(GetPosition(), size), GUI.color);
    }

}
