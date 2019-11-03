# 简单的鼠标打飞碟/打靶游戏

用 Unity 实现的游戏，采用纯代码生成所有游戏对象的方法构建游戏。

游戏采用 MVC 模式编写。

## 飞碟游戏

### 视频

[优酷视频](https://v.youku.com/v_show/id_XNDM4ODM2ODkwOA==.html?spm=a2h3j.8428770.3416059.1)

[视频下载地址](https://github.com/huanghongxun/3D-Programming-And-Design/tree/master/homework4/Hit-UFO/Hit-UFO.mp4)

### 规则

1. 一轮 10 秒，第 i 轮发射 i 个飞碟
2. 有圆盘和圆球两种飞碟
3. 点击飞碟得分，圆球得分少，圆盘得分多，颜色不同得分不同
4. 飞碟落地不得分
5. 随着游戏进行，飞碟移动距离和速度都将变大，移动距离和速度在范围内随机
6. 若第 i 轮没有拿到 i 分则失败

### 代码

#### Action

1. `ExplodeAction`：触发游戏对象爆炸的动作
2. `TipAction`：游戏对象上方显示分数的动作
3. `MoveAction`：位移动作，用于基础运动管理器

#### Controller

1. `Entity`：Controller 的基类，处理点击事件
2. `EntityFactory`：负责创建 `Entity`
3. `GameController`：游戏控制器，负责联系游戏进程 `Game` 和 GUI `GuiIngame`
4. `UFO`：飞碟的控制器，管理飞碟的点击事件，并控制飞碟的模型和颜色
5. `EntityController`：监听点击事件，并发给 `Entity`
6. `SSDirector`：控制场景变换

#### Model

1. `Game`：负责控制游戏进程
2. `Ruler`：裁判类，负责控制一轮游戏的进程
3. `UFOModel`：飞碟，处理飞碟的飞行轨迹、速度，以及爆炸和提示得分
4. `IActionManager`：运动接口，负责发射飞碟
5. `CCActionManager`：基础运动类，负责通过基础运动发射飞碟
6. `PhysicalActionManager`：物理运动类，负责通过物理引擎发射飞碟

#### View

1. `GuiIngame`：显示轮次、得分、剩余飞碟数
2. `EntityRenderer`：游戏对象视图类的基类
3. `EntityRendererFactory`：负责游戏对象的创建和回收
4. `UFOEditor`：实现 `Component` 需求：自定义飞碟属性界面
5. `UFORenderer`：负责绘制飞碟，包括创建和回收飞碟游戏对象

### `IActionManager`

通过将 `Ruler` 中发射 UFO 的代码抽离成一个单独的类来实现发射飞碟。

```csharp
public interface IActionManager
{
    // 发射 UFO
    void SendUFO(Game game, Ruler ruler, int round);
    // 判断 UFO 是否掉出世界
    bool IsUFODead(UFO ufo);
}
```

### `CCActionManager`

`CCActionManager` 负责通过位移的方式发射飞碟。

其中该类通过创建一个 `MoveAction` 来控制飞碟的运动。

```csharp
public class CCActionManager : IActionManager
{
    public void SendUFO(Game game, Ruler ruler, int round)
    {
        float speed = 0.1f;
        for (int i = 1; i < round; ++i) speed *= 1.1f;

        float actualSpeed = Random.Range(speed, speed * 1.3f);

        UFO ufo = UFO.Factory.Instance.Instantiate(new UFOModel
        {
            score = Random.Range(1, 5),
            game = ruler
        });
        ufo.gameObject.transform.parent = game.transform;
        ufo.gameObject.transform.position = ufo.renderer.initialPosition;
        Vector3 dir = ufo.renderer.initialDirection.normalized * 30;
        dir.y /= 4;
        MoveAction action = ufo.gameObject.AddComponent<MoveAction>();
        action.MovePosition( ufo.renderer.initialDirection + dir);
        action.Duration /= actualSpeed * 10;
        action.StartAction();
    }

    public bool IsUFODead(UFO ufo)
    {
        return !ufo.gameObject.GetComponent<MoveAction>();
    }
}
```

### `PhysicalActionManager`

`PhysicalActionManager` 通过 Unity 的物理引擎来发射飞碟。

```csharp
public class Ruler
{
    private readonly int round;
    private readonly Game game;
    private float time = 0;

    public int Trial { get; private set; }

    public int MaxTrial { get; private set; } = 10;

    public int Score { get; private set; }

    public Ruler(Game game, int round)
    {
        this.game = game;
        this.round = round;
        this.Trial = this.MaxTrial = round;
        this.time = 10f / MaxTrial;
        this.Score = 0;
    }

    public void SendUFO()
    {
        var ufo = new UFOModel
        {
            score = Random.Range(1, 5),
            game = this
        };
        UFO ufoEntity = UFO.Factory.Instance.Instantiate(ufo);
        ufoEntity.gameObject.transform.parent = game.transform;

        float speed = 0.1f;
        for (int i = 1; i < round; ++i) speed *= 1.1f;

        float actualSpeed = Random.Range(speed, speed * 1.3f);
        Rigidbody body = ufo.Send(ufoEntity.renderer.initialPosition, ufoEntity.renderer.initialDirection, actualSpeed);
        // body.AddTorque(new Vector3(1, 0, 0) * 20);
    }

    public void Update()
    {
        time += Time.deltaTime;
        while (time >= 10f / MaxTrial && Trial > 0)
        {
            Trial--;
            time -= 10f / MaxTrial;
            SendUFO();
        }

        foreach (var ufo in game.GetComponentsInChildren<UFO>())
        {
            if (!ufo.model.success && ufo.transform.position.y < 0)
                EntityRendererFactory.Instance.Collect(ufo.gameObject);
        }

        if (Trial <= 0 && game.GetComponentsInChildren<UFO>().Length == 0)
        {
            if (Score < round)
            {
                Debug.Log(this.GetHashCode());
                game.RoundLose();
            }
            else
            {
                game.RoundWin();
            }
        }
    }

    public void AddScore(int score)
    {
        Debug.Log(this.GetHashCode());
        this.Score += score;
    }
}
```

## 打靶游戏

### 规则

1. 靶对象为 5 环，按环计分；
2. 箭对象，射中后要插在靶上
   * **增强要求**：射中后，箭对象产生颤抖效果，到下一次射击 或 1秒以后 
3. 游戏仅一轮，无限 trials；
   - **增强要求**：添加一个风向和强度标志，提高难度

### 代码

#### Action

1. `TipAction`：游戏对象上方显示分数的动作
2. `ShakeAction`：箭的颤抖动作

#### Controller

1. `Entity`：Controller 的基类，处理点击事件
2. `EntityFactory`：负责创建 `Entity`
3. `GameController`：游戏控制器，负责联系游戏进程 `Game` 和 GUI `GuiIngame`
4. `Arrow`：箭的控制器，接收箭的碰撞事件
5. `SSDirector`：控制场景变换

#### Model

1. `Game`：负责控制游戏进程
2. `Ruler`：裁判类，负责控制一轮游戏的进程
3. `ArrowModel`：箭，处理箭的飞行轨迹、速度、处理碰撞、瞄准

#### View

1. `GuiIngame`：显示轮次、得分、剩余飞碟数
2. `EntityRenderer`：游戏对象视图类的基类
3. `EntityRendererFactory`：负责游戏对象的创建和回收
4. `ArrowRenderer`：负责绘制箭，包括创建和回收箭游戏对象

## 颤抖

箭对象的颤抖很好做，通过 `transform.RotateAround` 方法就可以实现箭围绕箭头尖的坐标旋转。接下来的问题就是旋转角的函数了，经过尝试，我找到了这个函数：

$$ angle(t)=0.4^t\sin(s\log(t+1)) $$

其中，$s$ 为颤抖力度参数，$s$ 越大，颤抖频率越高，$t$ 为时间。函数图像如下：

![1571320204959](E:\sources\homework\3D-Programming-And-Design\homework5\assets\1571320204959-1571321360189.png)

可以看到旋转角随时间颤抖幅度越来越小，而且周期越来越长，最后趋于 0。

```csharp
time += Time.deltaTime;
if (time > duration) time = duration;
transform.localPosition = position;
transform.localRotation = rotation;

if (time >= duration)
{
    Destroy(this);
}
else
{
    transform.RotateAround(center, axis, angle * Mathf.Pow(0.4f, time) * Mathf.Sin(Mathf.Log(time + 1) * speed));
}
```

同时，我将角度和频率与箭发射力度关联了起来，箭速度越快，角度越大，频率越高。

```csharp
gameObject.AddComponent<ShakeAction>().Shake(point, Vector3.up, duration: 3.92f, speed: strength * 10, angle: strength / 20f + 0.5f);
```

### 截图

左上角为游戏信息，其中 Wind 为风力方向。

![1571157597193](E:/sources/homework/3D-Programming-And-Design/homework5/assets/1571157597193.png)

### 视频

[优酷视频](https://v.youku.com/v_show/id_XNDQwMjIzNzQ3Ng==.html?spm=a2h3j.8428770.3416059.1 )
[视频下载地址](https://github.com/huanghongxun/3D-Programming-And-Design/tree/master/homework5/Targeting-Game/record.mp4)