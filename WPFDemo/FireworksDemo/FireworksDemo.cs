namespace WPFDemo.FireworksDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Physics2D;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Force;
    using Physics2D.Force.Zones;
    using Physics2D.Object;
    using WPFDemo.Graphic;

    public class FireworksDemo : PhysicsGraphic, IDrawable
    {
        /// <summary>
        /// 选项状态枚举
        /// </summary>
        public enum PhysicsType
        {
            None,
            Water,
            Wind
        }

        /// <summary>
        /// 当前选项
        /// </summary>
        private PhysicsType type = PhysicsType.None;

        /// <summary>
        /// 当前选项
        /// </summary>
        public PhysicsType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;

                // 设置力场
                if (this.type == PhysicsType.Water)
                {
                    this.PhysicsWorld.Zones.Add(this.dragZone);
                    this.PhysicsWorld.Zones.Remove(this.windZone);
                }
                else if (this.type == PhysicsType.Wind)
                {
                    this.PhysicsWorld.Zones.Add(this.windZone);
                    this.PhysicsWorld.Zones.Remove(this.dragZone);
                }
            }
        }

        /// <summary>
        /// 阻力
        /// </summary>
        private readonly ParticleDrag drag = new ParticleDrag(2, 1);

        /// <summary>
        /// 风
        /// </summary>
        private readonly ParticleConstantForce wind = new ParticleConstantForce(new Vector2D(20, -5));

        /// <summary>
        /// 阻力区
        /// </summary>
        private readonly Zone dragZone;

        /// <summary>
        /// 有风区
        /// </summary>
        private readonly Zone windZone;

        /// <summary>
        /// 横板
        /// </summary>
        private Edge edge;

        /// <summary>
        /// 物体列表
        /// </summary>
        private readonly List<Particle> objList = new List<Particle>();

        /// <summary>
        /// 粒子形状Id
        /// 所有粒子都使用相同的Id，从而使粒子之间可以交叠
        /// </summary>
        private readonly int shapeId = Shape.NewId();

        /// <summary>
        /// 粒子半径
        /// </summary>
        private const int BallSize = 4;
        private const int WorldHeight = 400;
        private const int WorldWidth = 500;

        public FireworksDemo(Image image)
            : base(image)
        {
            this.DrawQueue.Add(this);

            this.dragZone = new RectangleZone(
                0.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                400.ToSimUnits());
            this.dragZone.Add(this.drag);

            this.windZone = new RectangleZone(
                0.ToSimUnits(),
                (WorldHeight * 1 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits());
            this.windZone.Add(this.wind);
        }

        protected override void UpdatePhysics(double duration)
        {
            this.PhysicsWorld.Update(duration);
        }

        public void Draw(WriteableBitmap bitmap)
        {
            if (this.type == PhysicsType.Water)
            {
                bitmap.FillRectangle(0, WorldHeight * 2 / 3, WorldWidth, WorldHeight, Colors.SkyBlue);
            }
            else if (this.type == PhysicsType.Wind)
            {
                bitmap.FillRectangle(0, WorldHeight * 1 / 3, WorldWidth, WorldHeight * 2 / 3, Colors.LightGray);
            }

            bitmap.DrawLineAa(
                this.edge.PointA.X.ToDisplayUnits(),
                this.edge.PointA.Y.ToDisplayUnits(),
                this.edge.PointB.X.ToDisplayUnits(),
                this.edge.PointB.Y.ToDisplayUnits(), Colors.Black);

            for (int i = this.objList.Count - 1; i >= 0; i--)
            {
                int x = this.objList[i].Position.X.ToDisplayUnits();
                int y = this.objList[i].Position.Y.ToDisplayUnits();

                if (y > WorldHeight || x > WorldWidth || x < 0 || y < 0)
                {
                    this.PhysicsWorld.RemoveObject(this.objList[i]);
                    this.objList.Remove(this.objList[i]);
                }
                else
                {
                    if (this.type == PhysicsType.Water)
                    {
                        bitmap.FillEllipseCentered(x, y, BallSize, BallSize, y > WorldHeight * 2 / 3 ? Colors.DarkBlue : Colors.Black);
                    }
                    else
                    {
                        bitmap.FillEllipseCentered(x, y, BallSize, BallSize, Colors.Black);
                    }
                }
            }
        }

        public void Fire(double x, double y)
        {
            if (!this.Start)
            {
                this.Start = true;

                // 增加重力
                this.PhysicsWorld.CreateGravity(9.8);

                // 添加边缘
                this.edge = this.PhysicsWorld.CreateEdge(
                    100.ToSimUnits(),
                    350.ToSimUnits(),
                    400.ToSimUnits(),
                    200.ToSimUnits());
                this.Slot = 1 / 60.0;
            }

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var paritcle = this.PhysicsWorld.CreateParticle(
                    new Vector2D(x, y),
                    new Vector2D(rnd.NextDouble() * 6 - 3, rnd.NextDouble() * 6 - 3),
                    1f, 0.1);
                paritcle.BindShape(new Circle(BallSize.ToSimUnits(), this.shapeId));
                this.objList.Add(paritcle);
            }
        }
    }
}