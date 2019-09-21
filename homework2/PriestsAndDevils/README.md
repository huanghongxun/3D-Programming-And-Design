# PriestsAndDevils

用 Unity 实现的游戏，采用纯代码生成所有游戏对象的方法构建游戏。

游戏采用 MVC 模式编写。

## UML 图

1. Models
   1. Boat - 表示船的相关信息，比如船装了哪些 Character，船再哪个岸上，通过 GameObject 的属性层级关系表示
   2. Priest - 表示牧师
   3. Devil - 表示恶魔
   4. Coast - 表示河岸的相关信息，比如河岸上有哪些 Character，是否停靠了船，也通过 GameObject 的属性层级关系表示
2. Controller
   1. GameController - 控制游戏界面流程，比如创建游戏对象，检查游戏局面是赢是输
   2. SSDirector - 控制游戏场景切换，本游戏只有游戏主界面一个场景
   3. SceneController - 场景的控制器，控制场景的生命周期
   4. EntityController - 负责处理 View 发来的点击事件，将点击事件处理后触发 Model 的变化
3. View
   1. GuiIngame

![PlantUML Diagram](https://www.plantuml.com/plantuml/img/XL9B2eCm4Dtd55uWDoX5r5AwA1GymH133SGCJ4PBeOUlQFocqjsy7_D9Kiz0t3PM26N1-oH3u53cDH0erZHX13zfXUwC3VN5iU7t1kJecD3QENk5HiTaQsxksIi1FyZekKEfj8iZPLaOqegHX1XR9QUFhF3bv6hUI1actxF3gdDeuSCOFvHnwpiwp7GDdS5s9IxR71csDpXmHGSyyEtl8_R1HChpz3RRrd-KlAS8X4Yrg_gVyGK0)

## 演示视频

https://github.com/huanghongxun/3D-Programming-And-Design/tree/master/homework2/PriestsAndDevils/spotlight.mp4

