using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Ruler
{
    private static GameObject fence = Resources.Load<GameObject>("Prefabs/Fence");
    private static GameObject collider = Resources.Load<GameObject>("Prefabs/Collider");

    private readonly Game game;

    private Player player;
    private Enemy[,] enemies = new Enemy[3, 3];
    private GameObject root;

    public int Trial { get; private set; }

    public int MaxTrial { get; private set; } = 10;

    public int Score { get; private set; }

    public Ruler(Game game, int round)
    {
        this.game = game;
        this.Trial = this.MaxTrial = round;
        this.Score = 0;
    }

    public void Start()
    {
        root = new GameObject();

        // 1 表示通道，0 表示封闭
        var horizontal = new bool[4, 12];
        var vertical = new bool[4, 12];

        for (var i = 0; i < 4; ++i)
        for (var j = 0; j < 3; ++j)
        {
            if (1 <= i && i <= 2)
            {
                horizontal[i, j * 4 + Random.Range(0, 4)] = true;
                vertical[i, j * 4 + Random.Range(0, 4)] = true;
            }
        }


        for (var i = -1; i <= 1; ++i)
        for (var j = -1; j <= 1; ++j)
        {
            var center = new Vector3(i * 8, 0, j * 8);
            var obj = Region.Factory.Instance.Instantiate(new RegionModel
            {
                score = 1,
                x = i,
                y = j
            });
            obj.model.Collision += (sender, index) => {
                Trace(index.x, index.y);
                AddScore(1);
            };
            obj.transform.position = center;
            obj.transform.parent = root.transform;
            enemies[i + 1, j + 1] = Enemy.Factory.Instance.Instantiate(new EnemyModel
            {
                score = 1,
                speed = 1,
                // 通过在圆上随机选择 5 个点来构造一个凸五边形
                points = new Queue<Vector3>(Enumerable
                    .Repeat(0, 5)
                    .Select(x => Random.Range(0, 360))
                    .OrderBy(x => x)
                    .Select(angle => center + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * 3)
                    .ToArray())
            });
            enemies[i + 1, j + 1].Wander();
            enemies[i + 1, j + 1].transform.parent = root.transform;
        }

        for (var i = 0; i < 4; ++i)
        {
            for (var j = 0; j < 12; ++j)
            {
                if (!horizontal[i, j])
                {
                    var obj = Object.Instantiate(fence);
                    obj.transform.position = new Vector3(-11 + 2 * j, 0, -12 + i * 8);
                    obj.transform.parent = root.transform;
                }

                if (!vertical[i, j])
                {
                    var obj = Object.Instantiate(fence);
                    obj.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    obj.transform.position = new Vector3(-12 + i * 8, 0, -11 + 2 * j);
                    obj.transform.parent = root.transform;
                }
            }
        }

        player = Player.Factory.Instance.Instantiate(new PlayerModel
        {
            game = this,

        });
        player.transform.position = new Vector3(5, 0, 5);
        player.transform.parent = root.transform;
    }

    public void Update()
    {
    }

    public void Trace(int x, int y)
    {
        for (var i = 0; i < 3; ++i)
        for (var j = 0; j < 3; ++j)
            enemies[i, j].Wander();
        enemies[x + 1, y + 1].Trace(player.gameObject, 1);
    }

    public void AddScore(int score)
    {
        Debug.Log(this.GetHashCode());
        this.Score += score;
    }

    public void Over()
    {
        game.RoundLose();
    }

    public void OnDestroy()
    {
        Object.Destroy(root);
    }
}
