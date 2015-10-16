Physics2D
=========

一个使用C#编写的2D质体物理引擎，项目包含了

  - 质体物理引擎
  - 单元测试
  - 基于WPF绘制的简单Demo合集

该项目是一个实验项目(Toy Project)，用于实践我所学习到的关于框架设计、语言特性等知识，因此该项目不具备工业项目的稳定性及性能(说到性能，其实用C#做这样的项目就不算太合适)，但是制作一些基于WPF的小型游戏应该还是可以做到的。为了简化系统自身，并且基于项目的实现目的，该引擎并没有考虑实现刚体物理模型，但是各个模块的设计为实现刚体物理模型提供了可能性和便利性，也就是说你可以很容易的将刚体物理模型加入到本引擎当中。

## 功能(包含计划中的)

  - [x] 质体(粒子)
  - [x] 可以定制的质体作用力发生器
  - [x] 可以定制的区域作用力
  - [x] 质体碰撞处理
  - [x] 质体连杆、绳索
  - [ ] 摩擦力
  - [ ] 液体模拟
  - [ ] 沙盒模拟器

## 开始使用

基本使用
```csharp
// 创建一个物理世界
var world = new World();

// 在物理世界里创建一个质体(粒子)
// 位置 (0, 0) 初速度 (1, 0) 质量 1 kg
var particle = world.CreateParticle(Vector2D.Zero, new Vector2D(1, 0), 1);

// 创建一个全局有效的重力场
var zone = world.CreateGlobalZone(new ParticleGravity(new Vector2D(0, 9.8));

// 执行 1/60 s
world.Update(1/60.0);
```

添加碰撞
```csharp
// 创建一条长5m深4m的底边, 碰撞恢复系数为0.5
var edge = world.CreateEdge(0.5, 0, 4, 5, 4);

// 将质点视为可与底边接触的球(圆)，半径为0.2m
edge.AddBall(particle, 0.2);

// 执行 1/60 s
world.Update(1/60.0);
```
  
## 分支说明

目前该项目的Demo图形依赖于WriteableBitmapEx(通过nuget引入到项目)，限于GDI+的性能问题，也希望能够尝试使用SharpDX或Win2D这样的由DirectX驱动的图形库，为此我创建了两个分支：

  - SharpDXonWPF
  在WPFDemo项目中增加了基于SharpDX中Direct2D的Image扩展，使用Direct2D作为渲染驱动。该尝试仍处于试验状态。

  - UWPDemo
  由于Win2D仅支持UWP等沙盒内应用，所以增加了UWPDemo项目，由于将Physics2D项目更改为了PCL类型，所以Physics2D可以同时支持WPF以及UWP应用。还未正式启动该项尝试。

如果你喜欢相关的技术，欢迎为相关的分支贡献代码。
  
## 演示动画

  由于图片比较大，加载完毕可能需要一定时间

  - 烟花

![](https://github.com/Blueve/Physics2D/blob/master/Images/firework.gif)

  - 圆周运动

![](https://github.com/Blueve/Physics2D/blob/master/Images/cycle.gif)

  - 弹性材料

![](https://github.com/Blueve/Physics2D/blob/master/Images/spring.gif)
    
  - 变形球

![](https://github.com/Blueve/Physics2D/blob/master/Images/metaball.gif)

  - 牛顿摆

![](https://github.com/Blueve/Physics2D/blob/master/Images/newton_scradle.gif)

  - 多边形连杆（模拟刚体）

![](https://github.com/Blueve/Physics2D/blob/master/Images/poly_rod.gif)