namespace WPFDemo.RobDemo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Physics2D;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Object;
    using Physics2D.Object.Tools;
    using WPFDemo.Graphic;

    internal class RobDemo : PhysicsGraphic, IDrawable
    {
        /// <summary>
        /// 多边形顶点集
        /// </summary>
        private readonly List<Vector2D> vertexs = new List<Vector2D>
        {
            new Vector2D(200, 20).ToSimUnits(),
            new Vector2D(300, 40).ToSimUnits(),
            new Vector2D(300, 100).ToSimUnits()
        };

        /// <summary>
        /// 多边形边集
        /// </summary>
        private readonly List<Vector2D> edgePoints = new List<Vector2D>
        {
            new Vector2D(10, 10).ToSimUnits(),
            new Vector2D(490, 10).ToSimUnits(),
            new Vector2D(490, 390).ToSimUnits(),
            new Vector2D(10, 390).ToSimUnits()
        };

        /// <summary>
        /// 组合质体
        /// </summary>
        private readonly CombinedParticle combinedParticle;

        /// <summary>
        /// 当前状态
        /// </summary>
        private State state = State.Up;

        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Vector2D mousePosition = Vector2D.Zero;

        /// <summary>
        /// 固定点操纵杆
        /// </summary>
        private Handle handle;

        /// <summary>
        /// 状态枚举
        /// </summary>
        private enum State
        {
            Down, Pinned, Up
        }

        public RobDemo(Image image)
            : base(image)
        {
            Settings.ContactIteration = 20;

            this.combinedParticle = new CombinedParticle(this.vertexs, 3, 1, true);
            this.PhysicsWorld.AddObject(this.combinedParticle);

            // 为顶点绑定形状
            foreach (var vertex in this.combinedParticle.Vertexs)
            {
                vertex.BindShape(new Circle(4.ToSimUnits()));
            }

            // 增加边缘
            this.PhysicsWorld.CreatePolygonEdge(this.edgePoints.ToArray());

            // 增加重力
            this.PhysicsWorld.CreateGravity(9.8);

            this.DrawQueue.Add(this);
            this.Start = true;
        }

        protected override void UpdatePhysics(double duration)
        {
            if (this.state == State.Pinned)
            {
                var d = this.mousePosition - this.handle.Position;
                this.combinedParticle.Velocity = d / duration;
                this.handle.Position = this.mousePosition;
            }

            this.PhysicsWorld.Update(duration);
        }

        public void Down(double x, double y)
        {
            this.mousePosition.Set(x, y);

            var points = from v in this.combinedParticle.Vertexs
                         select v.Position;
            if (this.state != State.Down && MathHelper.IsInside(points.ToList(), this.mousePosition))
            {
                this.handle = this.PhysicsWorld.Pin(this.combinedParticle, this.mousePosition);
                this.state = State.Pinned;
            }
            else
            {
                this.state = State.Down;
            }
        }

        public void Move(double x, double y)
        {
            if (this.state == State.Pinned)
            {
                this.mousePosition.Set(x, y);
            }
        }

        public void Up()
        {
            if (this.state == State.Pinned)
            {
                this.PhysicsWorld.UnPin(this.combinedParticle);
            }

            this.state = State.Up;
        }

        public void Draw(WriteableBitmap bitmap)
        {
            // 绘制物体
            var points = new List<int>();
            foreach (var vertex in this.combinedParticle.Vertexs)
            {
                points.Add(vertex.Position.X.ToDisplayUnits());
                points.Add(vertex.Position.Y.ToDisplayUnits());
            }

            bitmap.FillPolygon(points.ToArray(), Colors.LightCoral);

            // 绘制边缘
            points.Clear();
            foreach (var point in this.edgePoints)
            {
                points.Add(point.X.ToDisplayUnits());
                points.Add(point.Y.ToDisplayUnits());
            }

            points.Add(this.edgePoints[0].X.ToDisplayUnits());
            points.Add(this.edgePoints[0].Y.ToDisplayUnits());
            bitmap.DrawPolyline(points.ToArray(), Colors.Black);

            // 绘制PinRod
            foreach (var e in this.combinedParticle.PinRods)
            {
                bitmap.DrawLineAa(
                    e.ParticleA.Position.X.ToDisplayUnits(), e.ParticleA.Position.Y.ToDisplayUnits(),
                    e.ParticleB.Position.X.ToDisplayUnits(), e.ParticleB.Position.Y.ToDisplayUnits(), Colors.Red);
            }
        }
    }
}
