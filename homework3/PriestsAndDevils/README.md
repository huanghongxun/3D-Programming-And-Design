# PriestsAndDevils

用 Unity 实现的游戏，采用纯代码生成所有游戏对象的方法构建游戏。

游戏采用 MVC 模式编写。

## Skybox

通过运用 Fantasy SkyBox FREE，我创建了河流、沙滩、树林、云层、月亮如下：

![1569514218011](E:\sources\homework\3D-Programming-And-Design\homework3\PriestsAndDevils\assets\1569514218011.png)

### 游戏对象的使用

游戏对象表示场景中的一个物体，从预先制作的模型预设实例化产生。预设不仅包含模型数据，还可以包含预先配置的 Component 及其配置参数。这样实例化的游戏对象就可以直接拥有自己的行为了。

## UML 图

1. Model
   1. BoatModel - 表示船的相关信息，比如船装了哪些 GameObject，支持上下船的动作
   2. CoastModel - 表示河岸的相关信息，比如河岸上有哪些 Character，是否停靠了船，通过 GameObject 的层次表示（Boat 的 gameObject 是 Coast 的 gameObject 的 child 表示 Boat 停靠了这个 Coast）
   3. Game - 游戏的裁判类，保存游戏局面，并在 Model 发生改变时计算游戏状态（GameController 监听后会通知 GameController 游戏失败）
2. Controller - 每个 GameObject 都会有自己的 Controller
   1. Entity - 所有游戏对象的 Controller 的基类，本游戏中负责处理鼠标点击事件
   2. Boat - 负责接收船的点击事件，并要求 BoatModel 处理上下船的动作
   3. Priest - 表示牧师
   4. Devil - 表示恶魔
   5. Coast - 负责接收河岸的点击事件，并要求 CoastModel 处理上下岸的动作
   6. GameController - 控制游戏界面流程，比如创建游戏对象，在游戏局面变化后通知 View 显示游戏状态

   7. SSDirector - 控制游戏场景切换，本游戏只有游戏主界面一个场景
   8. SceneController - 场景的控制器，控制场景的生命周期
   9. EntityController - 负责处理 View 发来的点击事件，将点击事件处理发送给对应的 Controller 处理
3. View

   1. GuiIngame - 绘制界面按钮及文字
   2. BoatRenderer - 船的 View
   3. CoastRenderer - 岸的 View
4. Action

   1. MoveAction - 牧师、魔鬼的移动动作脚本
   2. BoatMoveAction - 船的移动动作脚本

![PlantUML Diagram](https://www.plantuml.com/plantuml/img/XLDHQiGW4Ftt5Bc0DrZAThEbz2DGcXE86Rg2wnHZKWez_0PDrD6a_NFUcvaydo_dqGdhfhjcJ6enZiq5XSkE7GwWi_e3p00Mx0grDs65TAb6CRhydCzxe5XmXLbj_8jnolsGekWEO8l6MTGwmg_Y3ZKugJVpkJ1h_MuLH6sz15EFzFrLMP0EDw336PwbKsY89H5aGL1p-kK3VXkU-L4ntfaIzMx7eOIlOdUOO1ZhqE05Rc9ME7bs-kBDgghYmcGovAY1sdZGLRt1GLzfyP_huZlltYk4YD5MQc9SSDH4myVyocr_2Zg1im2pO6qI52D0YVry91SQpf-sd5Ex_s7Q1RA2iIZLeyNdA9dhI5UfgFFYoVB_Fm00)

## 动作分离

在船移动时，EntityController 首先接收到 Boat 的点击事件，然后通知 BoatController 处理 Boat 的点击事件，BoatController 将会调用 `Boat.StopAt` 函数发起船的移动动作。该函数的实现如下：

```csharp
public void StopAt(Coast coast, ActionStyle style = ActionStyle.Animation)
{
    transform.parent = coast.transform;
    var action = gameObject.AddComponent<BoatMoveAction>();
    action.Action += delegate { StoppedAt?.Invoke(this, EventArgs.Empty); };
    action.MovePosition(coast.BoatStopPosition, Quaternion.LookRotation(coast.BoatStopDirection), style);
    action.StartAction();
}
```

可以看到我们通过创建 `BoatMoveAction` 并注册到 `gameObject` 的 Component 中来实现动作的注册。在开始动作之后船的坐标以及行进方向将被 `BoatMoveAction` 控制。

`BoatMoveAction` 部分代码如下：

```csharp
public override void UpdateAction()
{
    var vmax = 1 / (4 / (float)Math.PI * AccelerationTime + Duration - 2 * AccelerationTime);
    var coe = 2 * AccelerationTime / (float)Math.PI;
    var rel = (target - source) * vmax;
    time += Time.deltaTime;
    if (time <= Duration)
    {
        rel *= ...;
    }
    else
    {
        rel = target - source;
    }
    var newPosition = source + rel;
    if (local) transform.localPosition = newPosition;
    else transform.position = newPosition;
    time += Time.deltaTime;
    if (time >= Duration)
    {
        Action?.Invoke(this, EventArgs.Empty);
        Destroy(this);
    }
}
```

可以看到 `BoatMoveAction` 和 `Boat` 其实无关。`BoatMoveAction` 并不知道自己要操作的对象是船，因此实现了动作分离。

## 裁判类

裁判类通过拿到 Model 并监听 Model 的变化事件来实现对游戏局面的掌控。可以看到 Game 类只依赖 Coast、Boat 等 Model 类，和 Controller 以及 View 无关。

```csharp
public class Game : MonoBehaviour
{
    public GameState state;
    public CoastModel westCoast, eastCoast;
    public BoatModel boat;

    public event EventHandler GameStateChanged;

    public void StartGame()
    {
        boat.StoppedAt += (sender, args) =>
        {
            if (args.coast == eastCoast)
                state = GameState.East;
            else if (args.coast == westCoast)
                state = GameState.West;
            CheckGameState();
        };
        state = GameState.East;
    }

    private void CheckGameState()
    {
        if (state != GameState.West && state != GameState.East) return;

        var west = westCoast.GetOnShore().Where(c => c != null);
        var westPriests = west.Count(c => c.GetComponent<Priest>());
        var westDevils = west.Count(c => c.GetComponent<Devil>());
        var east = eastCoast.GetOnShore().Where(c => c != null);
        var eastPriests = east.Count(c => c.GetComponent<Priest>());
        var eastDevils = east.Count(c => c.GetComponent<Devil>());
        var boat = this.boat.GetOnBoat().Where(c => c != null);
        var boatPriests = boat.Count(c => c.GetComponent<Priest>());
        var boatDevils = boat.Count(c => c.GetComponent<Devil>());

        if (state == GameState.East)
        {
            eastPriests += boatPriests;
            eastDevils += boatDevils;
        }
        else
        {
            westPriests += boatPriests;
            westDevils += boatDevils;
        }

        if (westPriests + westDevils == 6)
        {
            state = GameState.Win;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (westPriests < westDevils && westPriests > 0 ||
            eastPriests < eastDevils && eastPriests > 0)
        {
            state = GameState.Lose;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
```



## 演示视频

在线观看：https://v.youku.com/v_show/id_XNDM3Njk4MDU2OA==.html?spm=a2h3j.8428770.3416059.1

https://github.com/huanghongxun/3D-Programming-And-Design/tree/master/homework3/PriestsAndDevils/spotlight.mp4

