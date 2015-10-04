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


namespace WPFDemo.RobDemo
{
    class RobDemo : PhysicsGraphic, IDrawable
    {
        #region 三角形顶点
        private readonly List<Particle> _poly = new List<Particle>
        {
            new Particle {
                Position = (new Vector2D(200, 0)).ToSimUnits(),
                Mass = 1
            },
            new Particle
            {
                Position = (new Vector2D(300, 20)).ToSimUnits(),
                Mass = 1
            },
            new Particle
            {
                Position = (new Vector2D(300, 80)).ToSimUnits(),
                Mass = 1
            }
        };

        #endregion

        #region 边界 
        private readonly List<ParticleEdge> _edges = new List<ParticleEdge>
        {
            new ParticleEdge(1, 9.ToSimUnits(), 390.ToSimUnits(), 491.ToSimUnits(), 390.ToSimUnits()),
            new ParticleEdge(1, 10.ToSimUnits(), 10.ToSimUnits(), 10.ToSimUnits(), 391.ToSimUnits()),
            new ParticleEdge(1, 490.ToSimUnits(), 10.ToSimUnits(), 490.ToSimUnits(), 391.ToSimUnits())
        };
        #endregion

        private State _state = State.Up;

        enum State
        {
            Down, Up
        }

        private Vector2D _mousePosition = Vector2D.Zero;

        public RobDemo(Image image)
            : base(image)
        {
            Settings.ContactIteration = 20;
            
            for (int i = 0; i < _poly.Count; i++)
            {
                PhysicsWorld += _poly[i];
                for (int j = i + 1; j < _poly.Count; j++)
                {
                    PhysicsWorld.ContactGenerators.Add(new ParticleRod(_poly[i], _poly[j]));
                }
                _edges.ForEach(e => e.AddBall(_poly[i], 4.ToSimUnits()));
            }

            // 增加底部边缘
            _edges.ForEach(e => PhysicsWorld.ContactGenerators.Add(e));

            // 增加重力
            PhysicsWorld.CreateGlobalZone(new ParticleGravity(new Vector2D(0, 10)));

            DrawQueue.Add(this);
            Start = true;

        }

        protected override void UpdatePhysics(double duration)
        {
            if (_state == State.Down)
            {
                var length = Vector2D.Distance(_mousePosition, _poly[0].Position);
                var normal = (_mousePosition - _poly[0].Position).Normalize();

                _poly[0].AddForce(normal * length * 2 + -Vector2D.UnitY * 30);
            }

            PhysicsWorld.Update(duration);
        }

        public void Down(double x, double y)
        {
            _mousePosition.Set(x, y);
            _state = State.Down;
        }

        public void Move(double x, double y)
        {
            _mousePosition.Set(x, y);
        }

        public void Up()
        {
            _state = State.Up;
        }

        public void Draw(WriteableBitmap bitmap)
        {

            var points = new List<int>();
            
            for (int i = 0; i < _poly.Count; i++)
            {
                points.Add(_poly[i].Position.X.ToDisplayUnits());
                points.Add(_poly[i].Position.Y.ToDisplayUnits());
                //for (int j = i + 1; j < _poly.Count; j++)
                //{
                //    bitmap.DrawLineAa(
                //        _poly[i].Position.X.ToDisplayUnits(), _poly[i].Position.Y.ToDisplayUnits(),
                //        _poly[j].Position.X.ToDisplayUnits(), _poly[j].Position.Y.ToDisplayUnits(), Colors.Black);
                //}
            }
            points.Add(points[0]);
            points.Add(points[1]);
            bitmap.FillPolygon(points.ToArray(), Colors.Coral);

            foreach (var e in _edges)
            {
                bitmap.DrawLineAa(
                    e.PointA.X.ToDisplayUnits(), e.PointA.Y.ToDisplayUnits(),
                    e.PointB.X.ToDisplayUnits(), e.PointB.Y.ToDisplayUnits(), Colors.Black);
                bitmap.DrawLineAa(
                    e.PointA.X.ToDisplayUnits() + 1, e.PointA.Y.ToDisplayUnits() + 1,
                    e.PointB.X.ToDisplayUnits() + 1, e.PointB.Y.ToDisplayUnits() + 1, Colors.Black);
            }
        }
    }
}
