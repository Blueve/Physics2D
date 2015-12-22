using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Physics2D;
using Physics2D.Collision;
using Physics2D.Collision.Basic;
using Physics2D.Common;
using Physics2D.Object;
using Physics2D.Object.Tools;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Factories;

using WPFDemo.Graphic;
using WPFDemo.ContactDemo;
using Physics2D.Collision.Shapes;

namespace WPFDemo.RobDemo
{
    class RobDemo : PhysicsGraphic, IDrawable
    {
        #region 私有字段
        /// <summary>
        /// 多边形顶点集
        /// </summary>
        private readonly List<Vector2D> _vertexs = new List<Vector2D>
        {
            new Vector2D(200, 20).ToSimUnits(),
            new Vector2D(300, 40).ToSimUnits(),
            new Vector2D(300, 100).ToSimUnits()
        };
        /// <summary>
        /// 多边形边集
        /// </summary>
        private readonly List<Vector2D> _edgePoints = new List<Vector2D>
        {
            new Vector2D(10, 10).ToSimUnits(),
            new Vector2D(490, 10).ToSimUnits(),
            new Vector2D(490, 390).ToSimUnits(),
            new Vector2D(10, 390).ToSimUnits()
        };
        /// <summary>
        /// 组合质体
        /// </summary>
        private readonly CombinedParticle _combinedParticle;
        /// <summary>
        /// 当前状态
        /// </summary>
        private State _state = State.Up;
        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Vector2D _mousePosition = Vector2D.Zero;
        /// <summary>
        /// 固定点操纵杆
        /// </summary>
        private Handle _handle;
        #endregion

        #region 内部类型
        /// <summary>
        /// 状态枚举
        /// </summary>
        enum State
        {
            Down, Pinned, Up
        }
        #endregion

        #region 构造方法
        public RobDemo(Image image)
            : base(image)
        {
            Settings.ContactIteration = 20;

            _combinedParticle = new CombinedParticle(_vertexs, 3, 1, true);
            PhysicsWorld.AddObject(_combinedParticle);

            // 为顶点绑定形状
            foreach(var vertex in _combinedParticle.Vertexs)
            {
                vertex.BindShape(new Circle(4.ToSimUnits()));
            }

            // 增加边缘
            PhysicsWorld.CreatePolygonEdge(_edgePoints.ToArray());

            // 增加重力
            PhysicsWorld.CreateGravity(9.8);

            DrawQueue.Add(this);
            Start = true;
        }
        #endregion

        #region 实现PhysicsGraphic
        protected override void UpdatePhysics(double duration)
        {
            if(_state == State.Pinned)
            {
                var d = _mousePosition - _handle.Position;
                _combinedParticle.Velocity = d / duration;
                _handle.Position = _mousePosition;
            }

            PhysicsWorld.Update(duration);
        }
        #endregion

        #region 公开方法
        public void Down(double x, double y)
        {
            _mousePosition.Set(x, y);

            var points = from v in _combinedParticle.Vertexs
                         select v.Position;
            if (_state != State.Down && MathHelper.IsInside(points.ToList(), _mousePosition))
            {
                _handle = PhysicsWorld.Pin(_combinedParticle, _mousePosition);
                _state = State.Pinned;
            }
            else
                _state = State.Down;
        }

        public void Move(double x, double y)
        {
            if(_state == State.Pinned)
                _mousePosition.Set(x, y);
        }

        public void Up()
        {
            if (_state == State.Pinned)
                PhysicsWorld.UnPin(_combinedParticle);
            _state = State.Up;
        }
        #endregion

        #region 实现IDrawable
        public void Draw(WriteableBitmap bitmap)
        {
            // 绘制物体
            var points = new List<int>();
            foreach(var vertex in _combinedParticle.Vertexs)
            {
                points.Add(vertex.Position.X.ToDisplayUnits());
                points.Add(vertex.Position.Y.ToDisplayUnits());
            }
            bitmap.FillPolygon(points.ToArray(), Colors.LightCoral);

            // 绘制边缘
            points.Clear();
            foreach(var point in _edgePoints)
            {
                points.Add(point.X.ToDisplayUnits());
                points.Add(point.Y.ToDisplayUnits());
            }
            points.Add(_edgePoints[0].X.ToDisplayUnits());
            points.Add(_edgePoints[0].Y.ToDisplayUnits());
            bitmap.DrawPolyline(points.ToArray(), Colors.Black);

            // 绘制PinRod
            foreach (var e in _combinedParticle.PinRods)
            {
                bitmap.DrawLineAa(
                    e.ParticleA.Position.X.ToDisplayUnits(), e.ParticleA.Position.Y.ToDisplayUnits(),
                    e.ParticleB.Position.X.ToDisplayUnits(), e.ParticleB.Position.Y.ToDisplayUnits(), Colors.Red);
            }
        }
        #endregion
    }
}
