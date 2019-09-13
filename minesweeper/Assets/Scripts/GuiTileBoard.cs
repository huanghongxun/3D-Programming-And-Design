using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏逻辑
/// </summary>
public class GuiTileBoard : GuiComponent
{
    public enum GameState
    {
        Uninitialized,
        Playing,
        Win,
        Lose
    }

    public Vector2Int gridSize { get; set; }
    public int totalMines { get; set; }

    public TileScript images { get; set; }

    public GuiTile[,] tiles;
    public bool[,] mines;
    public GameState state = GameState.Uninitialized;

    public GuiTileBoard(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
        tiles = new GuiTile[gridSize.x, gridSize.y];
        mines = new bool[gridSize.x, gridSize.y];

        for (int i = 0; i < gridSize.x; ++i)
            for (int j = 0; j < gridSize.y; ++j)
            {
                Vector2 pos = new Vector2(i * 30, j * 30);
                tiles[i, j] = new GuiTile(this, new Vector2Int(i, j))
                {
                    position = () => pos,
                    size = new Vector2(30, 30),
                    alignment = Alignment.TopLeft
                };
                tiles[i, j].MouseClicked += (sender, args) =>
                {
                    if (state == GameState.Uninitialized)
                        PlaceMines(i, j);
                };
                AddChild(tiles[i, j]);
            }
    }

    public void PlaceMines(int posX, int posY)
    {
        IList<Vector2Int> tileList = new List<Vector2Int>();
        for (int i = 0; i < gridSize.x; i++)
            for (int j = 0; j < gridSize.y; j++)
            {
                // 防止第一次点击点到炸弹，我们在第一次点击之后标记地雷
                // 第一次点击周围 9 个格子均不放置地雷
                if (posX - 1 <= i && i <= posX + 1 &&
                    posY - 1 <= j && j <= posY + 1)
                    continue;
                tileList.Add(new Vector2Int(i, j));
            }

        // 选择并放置地雷
        tileList.Shuffle();
        for (int i = 0; i < totalMines; ++i)
        {
            Vector2Int r = tileList[i];
            mines[r.x, r.y] = true;
        }

        state = GameState.Playing;
    }

    public int GetMines(Vector2Int position)
    {
        int totalMines = 0;
        for (int i = Math.Max(0, position.x - 1); i <= position.x + 1 && i < gridSize.x; ++i)
            for (int j = Math.Max(0, position.y - 1); j <= position.y + 1 && j < gridSize.x; ++j)
                if (mines[i, j]) ++totalMines;
        return totalMines;
    }

    /// <summary>
    /// 点击 pos 位置的方块
    /// 如果遇到周围没有炸弹的方块，那么将连片点击
    /// </summary>
    /// <param name="pos">被点击的方块</param>
    public void Reveal(Vector2Int pos)
    {
        if (state == GameState.Uninitialized)
            PlaceMines(pos.x, pos.y);

        tiles[pos.x, pos.y].state = GuiTile.TileState.Revealed;

        if (mines[pos.x, pos.y])
            Lose();
        else if (bombsLeft <= 0)
            Win();

        if (GetMines(pos) == 0)
        {
            for (int i = Math.Max(0, pos.x - 1); i <= pos.x + 1 && i < gridSize.x; ++i)
                for (int j = Math.Max(0, pos.y - 1); j <= pos.y + 1 && j < gridSize.y; ++j)
                {
                    if (i == pos.x && j == pos.y) continue;
                    if (tiles[i, j].state == GuiTile.TileState.Normal)
                        Reveal(new Vector2Int(i, j));
                }
        }
    }

    /// <summary>
    /// 表示鼠标中键点击方块的行为
    /// 如果方块周围的旗子数和炸弹数一致则点开周围未被点开的方块
    /// 如果旗子插错了此处将触发游戏失败
    /// </summary>
    /// <param name="pos">鼠标中键点击的方块</param>
    public void RevealAround(Vector2Int pos)
    {
        int mines = GetMines(pos), flags = 0;
        for (int i = Math.Max(0, pos.x - 1); i <= pos.x + 1 && i < gridSize.x; ++i)
            for (int j = Math.Max(0, pos.y - 1); j <= pos.y + 1 && j < gridSize.y; ++j)
                if (tiles[i, j].state == GuiTile.TileState.Flagged)
                    flags++;
        if (mines == flags)
        {
            for (int i = Math.Max(0, pos.x - 1); i <= pos.x + 1 && i < gridSize.x; ++i)
                for (int j = Math.Max(0, pos.y - 1); j <= pos.y + 1 && j < gridSize.y; ++j)
                    if (tiles[i, j].state == GuiTile.TileState.Normal)
                        Reveal(new Vector2Int(i, j));
        }
    }

    /// <summary>
    /// 还剩下的炸弹数 = 总炸弹数 - 旗子数
    /// </summary>
    public int bombsLeft
    {
        get
        {
            int bombs = totalMines;
            for (int i = 0; i < gridSize.x; i++)
                for (int j = 0; j < gridSize.y; j++)
                    if (tiles[i, j].state == GuiTile.TileState.Flagged)
                        bombs--;

            return bombs;
        }
    }

    /// <summary>
    /// 计算当前局面是否胜利
    /// </summary>
    /// <returns>游戏是否胜利</returns>
    public bool IsWin()
    {
        int revealed = 0;
        for (int i = 0; i < gridSize.x; i++)
            for (int j = 0; j < gridSize.y; j++)
                if (tiles[i, j].state == GuiTile.TileState.Revealed)
                    ++revealed;

        return revealed + totalMines >= gridSize.x * gridSize.y;
    }

    /// <summary>
    /// 游戏失败，显示所有地雷的位置并冻结盘面
    /// </summary>
    public void Lose()
    {
        state = GameState.Lose;

        for (int i = 0; i < gridSize.x; i++)
            for (int j = 0; j < gridSize.y; j++)
            {
                if (tiles[i, j].state == GuiTile.TileState.Flagged
                    && !mines[i, j])
                {
                    tiles[i, j].state = GuiTile.TileState.WrongMine;
                }

                if (mines[i, j])
                    if (tiles[i, j].state == GuiTile.TileState.Revealed)
                        tiles[i, j].state = GuiTile.TileState.Dead;
                    else
                        tiles[i, j].state = GuiTile.TileState.Mine;
            }
    }

    /// <summary>
    /// 游戏成功，冻结盘面
    /// </summary>
    public void Win()
    {
        state = GameState.Win;
    }

    /// <summary>
    /// 查询是否在游戏过程中
    /// </summary>
    public bool isPlaying
    {
        get
        {
            return state == GameState.Uninitialized || state == GameState.Playing;
        }
    }
}
