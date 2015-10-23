using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Physics2D;
using Physics2D.Collision;
using Physics2D.Collision.Basic;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Object;
using WPFDemo.Graphic;
using Physics2D.Collision.Shapes;

namespace WPFDemo.FireworksDemo
{

    public class FireworksDemo : PhysicsGraphic, IDrawable
    {
        public enum PhysicsType
        {
            None,
            Water,
            Wind
        }

        public const int BallSize = 4;
        private PhysicsType _type = PhysicsType.None;
        public PhysicsType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                // 设置力场
                if (_type == PhysicsType.Water)
                {
                    PhysicsWorld.Zones.Add(_dragZone);
                    PhysicsWorld.Zones.Remove(_windZone);
                }
                else if (_type == PhysicsType.Wind)
                {
                    PhysicsWorld.Zones.Add(_windZone);
                    PhysicsWorld.Zones.Remove(_dragZone);
                }
            }
        }

        // 空间作用力
        private readonly ParticleDrag _drag = new ParticleDrag(2, 1);
        private readonly ParticleConstantForce _wind = new ParticleConstantForce(new Vector2D(20, -5));

        private readonly Zone _dragZone;
        private readonly Zone _windZone;

        // 粒子队列
        private readonly List<Particle> _objList = new List<Particle>();

        private const int WorldHeight = 400;
        private const int WorldWidth = 500;

        public FireworksDemo(Image image)
            : base(image)
        {
            DrawQueue.Add(this);

            _dragZone = new RectangleZone
            (
                0.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                400.ToSimUnits()
            );
            _dragZone.Add(_drag);

            _windZone = new RectangleZone
            (
                0.ToSimUnits(),
                (WorldHeight * 1 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits()
            );
            _windZone.Add(_wind);
        }

        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }

        public void Draw(WriteableBitmap bitmap)
        {
            
            if (_type == PhysicsType.Water)
                bitmap.FillRectangle(0, WorldHeight * 2 / 3, WorldWidth, WorldHeight, Colors.SkyBlue);
            else if (_type == PhysicsType.Wind)
                bitmap.FillRectangle(0, WorldHeight * 1 / 3, WorldWidth, WorldHeight * 2 / 3, Colors.LightGray);

            bitmap.DrawLineAa(
                _edge.PointA.X.ToDisplayUnits(),
                _edge.PointA.Y.ToDisplayUnits(),
                _edge.PointB.X.ToDisplayUnits(),
                _edge.PointB.Y.ToDisplayUnits(), Colors.Black);
            //bitmap.DrawLineAa(
            //    _edge1.PointA.X.ToDisplayUnits(),
            //    _edge1.PointA.Y.ToDisplayUnits(),
            //    _edge1.PointB.X.ToDisplayUnits(),
            //    _edge1.PointB.Y.ToDisplayUnits(), Colors.Black);
            //bitmap.DrawLineAa(
            //    _edge2.PointA.X.ToDisplayUnits(),
            //    _edge2.PointA.Y.ToDisplayUnits(),
            //    _edge2.PointB.X.ToDisplayUnits(),
            //    _edge2.PointB.Y.ToDisplayUnits(), Colors.Black);

            for (int i = _objList.Count - 1; i >= 0; i--)
            {
                int x = _objList[i].Position.X.ToDisplayUnits();
                int y = _objList[i].Position.Y.ToDisplayUnits();

                if (y > WorldHeight || x > WorldWidth || x < 0 || y < 0)
                {
                    PhysicsWorld.RemoveObject(_objList[i]);
                    _objList.Remove(_objList[i]);
                }
                else
                {
                    if(_type == PhysicsType.Water)
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
            if (!Start)
            {
                Start = true;
                // 增加重力
                PhysicsWorld.CreateGravity(9.8);
                // 添加边缘
                _edge = PhysicsWorld.CreateEdge(
                    100.ToSimUnits(),
                    350.ToSimUnits(),
                    400.ToSimUnits(),
                    200.ToSimUnits());
                //_edge = PhysicsWorld.CreateEdge(
                //    100.ToSimUnits(),
                //    200.ToSimUnits(),
                //    100.ToSimUnits(),
                //    300.ToSimUnits());
                //_edge1 = PhysicsWorld.CreateEdge(
                //    250.ToSimUnits(),
                //    200.ToSimUnits(),
                //    250.ToSimUnits(),
                //    300.ToSimUnits());
                //_edge2 = PhysicsWorld.CreateEdge(
                //    100.ToSimUnits(),
                //    300.ToSimUnits(),
                //    250.ToSimUnits(),
                //    300.ToSimUnits());
                Slot = 1 / 60.0;
            }

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var paritcle = PhysicsWorld.CreateParticle
                (
                    new Vector2D(x, y),
                    new Vector2D(rnd.NextDouble() * 6 - 3, rnd.NextDouble() * 6 - 3),
                    1f, 0.1, true
                );
                paritcle.BindShape(new Circle(BallSize.ToSimUnits()));
                _objList.Add(paritcle);
            }
        }

        private Edge _edge, _edge1, _edge2;
    }
}