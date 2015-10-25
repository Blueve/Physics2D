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
        #region 三角形顶点
        private readonly List<Particle> _poly = new List<Particle>
        {
            new Particle {
                Position = (new Vector2D(200, 0)).ToSimUnits(),
                Mass = 1,
                Restitution = 1
            },
            new Particle
            {
                Position = (new Vector2D(300, 20)).ToSimUnits(),
                Mass = 1,
                Restitution = 1
            },
            new Particle
            {
                Position = (new Vector2D(300, 80)).ToSimUnits(),
                Mass = 1,
                Restitution = 1
            }
        };

        private readonly List<Vector2D> _vertexs = new List<Vector2D>
        {
            new Vector2D(200, 0).ToSimUnits(),
            new Vector2D(300, 20).ToSimUnits(),
            new Vector2D(300, 80).ToSimUnits()
        };
        #endregion

        #region 边界 
        private readonly List<Edge> _edges = new List<Edge>
        {
            new Edge(9.ToSimUnits(), 390.ToSimUnits(), 491.ToSimUnits(), 390.ToSimUnits()),
            new Edge(10.ToSimUnits(), 10.ToSimUnits(), 10.ToSimUnits(), 391.ToSimUnits()),
            new Edge(490.ToSimUnits(), 10.ToSimUnits(), 490.ToSimUnits(), 391.ToSimUnits())
        };
        #endregion

        private readonly CombinedParticle _combinedParticle;

        private State _state = State.Up;

        enum State
        {
            Down, Up
        }

        private Vector2D _mousePosition = Vector2D.Zero;

        private Particle _pin;

        public RobDemo(Image image)
            : base(image)
        {
            Settings.ContactIteration = 20;

            _combinedParticle = new CombinedParticle(_vertexs, Vector2D.Zero, 3, 1, true);
            PhysicsWorld.AddCustomObject(_combinedParticle);

            // 为顶点绑定形状
            foreach(var vertex in _combinedParticle.Vertexs)
            {
                vertex.BindShape(new Circle(4.ToSimUnits()));
            }

            // 增加边缘
            _edges.ForEach(e => PhysicsWorld.AddShape(e));

            // 增加重力
            PhysicsWorld.CreateGravity(9.8);

            DrawQueue.Add(this);
            Start = true;

        }

        protected override void UpdatePhysics(double duration)
        {
            if(_state == State.Down)
            {
                var d = _mousePosition - _pin.Position;
                _combinedParticle.Position = d;
                _pin.Position = _mousePosition;
            }

            PhysicsWorld.Update(duration);
        }

        public void Down(double x, double y)
        {
            _mousePosition.Set(x, y);
            

            var points = from v in _combinedParticle.Vertexs
                         select v.Position;
            if (MathHelper.IsInside(points.ToList(), _mousePosition))
            {
                _pin = _combinedParticle.Pin(PhysicsWorld, _mousePosition);
                _state = State.Down;
            }
        }

        public void Move(double x, double y)
        {
            _mousePosition.Set(x, y);
        }

        public void Up()
        {
            _state = State.Up;
            _combinedParticle.UnPin(PhysicsWorld);
        }

        public void Draw(WriteableBitmap bitmap)
        {
            var points = new List<int>();
            foreach(var vertex in _combinedParticle.Vertexs)
            {
                points.Add(vertex.Position.X.ToDisplayUnits());
                points.Add(vertex.Position.Y.ToDisplayUnits());
            }
            bitmap.FillPolygon(points.ToArray(), Colors.LightCoral);

            foreach (var e in _edges)
            {
                bitmap.DrawLineAa(
                    e.PointA.X.ToDisplayUnits(), e.PointA.Y.ToDisplayUnits(),
                    e.PointB.X.ToDisplayUnits(), e.PointB.Y.ToDisplayUnits(), Colors.Black);
                bitmap.DrawLineAa(
                    e.PointA.X.ToDisplayUnits() + 1, e.PointA.Y.ToDisplayUnits() + 1,
                    e.PointB.X.ToDisplayUnits() + 1, e.PointB.Y.ToDisplayUnits() + 1, Colors.Black);
            }

            foreach (var e in _combinedParticle.PinRods)
            {
                bitmap.DrawLineAa(
                    e.PA.Position.X.ToDisplayUnits(), e.PA.Position.Y.ToDisplayUnits(),
                    e.PB.Position.X.ToDisplayUnits(), e.PB.Position.Y.ToDisplayUnits(), Colors.Red);
            }
        }
    }
}
