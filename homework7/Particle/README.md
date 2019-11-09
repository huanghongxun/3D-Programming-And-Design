# 粒子系统

视频下载链接



## 汽车尾气模拟

### 创建粒子系统

根据官方教程，导入 Standard Assets 中的 Car 和 Smoke 如下：

![image-20191109231153602](assets/image-20191109231153602.png)

我们可以调整烟雾的 Emission 的 RateOverTime 来调整烟雾的浓度，来实现汽车普通情况和故障情况下烟雾浓度的不同：

![image-20191109231308105](assets/image-20191109231308105.png)

可以通过调整 Force Over LifeTime 来设置烟雾的方向、速度和传播距离，通过调整该项可以让烟雾间断喷出，形成故障效果

![image-20191109231415479](assets/image-20191109231415479.png)

选中 Size Over LifeTime 的 Curve，可以调整烟雾的半径，从而让”尾气“半径符合汽车的排气孔大小：

![image-20191109232104417](assets/image-20191109232104417.png)

最终效果如下：

![image-20191109232237778](assets/image-20191109232237778.png)