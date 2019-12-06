# PriestsAndDevils

用 Unity 实现的游戏，采用纯代码生成所有游戏对象的方法构建游戏。

游戏采用 MVC 模式编写。

## 截图

![1569514218011](E:\sources\homework\3D-Programming-And-Design\homework3\PriestsAndDevils\assets\1569514218011.png)

## 状态图生成

我通过 `GameStates` 实现状态图的生成。

实现状态图的方式很简单，利用深度优先搜索就可以找出从各个有效状态到达胜利状态的方式。只要从胜利状态往回倒着搜索，就可以得到各个状态的状态转移。

```csharp
public GameState Search(GameState currentState)
{
    if (states.ContainsKey(currentState)) return states[currentState];
    states[currentState] = currentState;

    foreach (GameAction action in GameAction.GameActions)
    {
        if (action.toWest && currentState.boatState == BoatState.East ||
            action.toEast && currentState.boatState == BoatState.West)
        {
            GameState originalState = currentState.Transform(action);
            if (originalState == null || !originalState.valid || originalState.lose)
                continue;
            GameState nextState = Search(originalState);
            nextState.nextWinAction = action.Opposite;
        }
    }

    return currentState;
}
```

## 图的表示法

在这里，我们只保存每个状态转移到目标状态的一种转移方法，所以实际上只保存下一个状态，因此整个状态图其实是个树，不存在环。每个点可能有多个入边，但只有一个出边。我们只需要保存每个状态的转移方法（即往哪个方向转移哪两个角色），就可以知道下一个状态。我们通过一个哈希集合的结构来保存和索引所有可达目标状态的有效状态。

```csharp
/// <summary>
/// 游戏动作，表示一次船的运输过程
/// 牧师和魔鬼的增减是相对于东岸的，也就是说，东岸牧师和魔鬼数均为 0 时游戏成功
/// </summary>
public sealed class GameAction
{
    public static readonly List<GameAction> GameActions = new List<GameAction>();

    static GameAction()
    {
        for (var priests = 0; priests <= 2; priests++)
            for (var devils = 0; devils <= 2; devils++)
            {
                if (priests + devils < 1 || priests + devils > 2) continue;
                GameActions.Add(new GameAction(priests, devils));
                GameActions.Add(new GameAction(-priests, -devils));
            }
    }

    public int priestDifference { get; }
    public int devilDifference { get; }

    public bool toEast => priestDifference >= 0 && devilDifference >= 0;
    public bool toWest => priestDifference <= 0 && devilDifference <= 0;
    public bool valid => Math.Abs(priestDifference + devilDifference) == 2 && priestDifference * devilDifference > 0;

    public GameAction(int priestDifference, int devilDifference)
    {
        this.priestDifference = priestDifference;
        this.devilDifference = devilDifference;
    }

    public GameAction Opposite => new GameAction(-priestDifference, -devilDifference);
}

public class GameState
{
    public int westPriests { get; }
    public int westDevils { get; }
    public int eastPriests { get; }
    public int eastDevils { get; }
    public BoatState boatState { get; }

    internal GameAction nextWinAction;

    public GameState(BoatState state, int westPriests, int westDevils, int eastPriests, int eastDevils)
    {
        this.boatState = state;
        this.westPriests = westPriests;
        this.westDevils = westDevils;
        this.eastPriests = eastPriests;
        this.eastDevils = eastDevils;
    }

    public bool lose =>
        westPriests < westDevils && westPriests > 0 ||
        eastPriests < eastDevils && eastPriests > 0;

    public bool valid =>
        westDevils >= 0 && westPriests >= 0 && eastDevils >= 0 && eastPriests >= 0 &&
        westDevils + westPriests + eastDevils + eastPriests == 6;

    public bool win => westDevils + westPriests == 6;

    public GameState Transform(GameAction action)
    {
        var nextState = new GameState(
            boatState == BoatState.East ? BoatState.West : BoatState.East,
            westPriests - action.priestDifference,
            westDevils - action.devilDifference,
            eastPriests + action.priestDifference,
            eastDevils + action.devilDifference);
        if (nextState.westPriests < 0 || nextState.westDevils < 0 ||
            nextState.eastPriests < 0 || nextState.eastDevils < 0)
            return null;
        else
            return nextState;
    }

    public bool Equals(GameState other)
    {
        return boatState == other.boatState && westPriests == other.westPriests && westDevils == other.westDevils && eastPriests == other.eastPriests && eastDevils == other.eastDevils;
    }

    public override bool Equals(object obj)
    {
        return obj is GameState other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = (int)boatState;
        hashCode = (hashCode * 397) ^ westPriests;
        hashCode = (hashCode * 397) ^ westDevils;
        hashCode = (hashCode * 397) ^ eastPriests;
        hashCode = (hashCode * 397) ^ eastDevils;
        return hashCode;
    }
}
```



## 演示视频

在线观看：http://v.youku.com/v_show/id_XNDQ2MDY1ODQwMA==.html?spm=a2h3j.8428770.3416059.1 

https://github.com/huanghongxun/3D-Programming-And-Design/tree/master/homework9/PriestsAndDevils/spotlight.mp4

