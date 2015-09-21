using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Physics2D;
using Physics2D.Collision;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Object;
using WPFDemo.Graphic;

namespace WPFDemo.FireworksDemo
{

    public class FireworksDemo : PhysicsGraphic, IDrawable
    {
        public const int WaterG = 1;
        public const int WindG = 2;

        private int _type;
        public int Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                // 设置力场
                if (_type == WaterG)
                {
                    if (!PhysicsWorld.ZoneSet.Contains(_dragZone))
                        PhysicsWorld.ZoneSet.Add(_dragZone);
                    if (PhysicsWorld.ZoneSet.Contains(_windZone))
                        PhysicsWorld.ZoneSet.Remove(_windZone);
                }
                else if (_type == WindG)
                {
                    if (!PhysicsWorld.ZoneSet.Contains(_windZone))
                        PhysicsWorld.ZoneSet.Add(_windZone);
                    if (PhysicsWorld.ZoneSet.Contains(_dragZone))
                        PhysicsWorld.ZoneSet.Remove(_dragZone);
                }
            }
        }

        // 空间作用力
        private readonly ParticleGravity _g = new ParticleGravity(new Vector2D(0, 10));
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
            _dragZone.ParticleForceGenerators.Add(_drag);

            _windZone = new RectangleZone
            (
                0.ToSimUnits(),
                (WorldHeight * 1 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits()
            );
            _windZone.ParticleForceGenerators.Add(_wind);
        }

        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }

        public void Draw(WriteableBitmap bitmap)
        {
            
            if (_type == WaterG)
                bitmap.FillRectangle(0, WorldHeight * 2 / 3, WorldWidth, WorldHeight, Colors.SkyBlue);
            else if (_type == WindG)
                bitmap.FillRectangle(0, WorldHeight * 1 / 3, WorldWidth, WorldHeight * 2 / 3, Colors.LightGray);

            bitmap.DrawLineAa(100, 350, 400, 200, Colors.Black);

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
                    if(_type == WaterG)
                    {
                        bitmap.FillEllipseCentered(x, y, 4, 4, y > WorldHeight*2/3 ? Colors.DarkBlue : Colors.Black);
                    }
                    else
                    {
                        bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
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
                PhysicsWorld.CreateGlobalZone(_g);
                // 添加边缘
                PhysicsWorld.RegistryContactGenerator(_contact);

                Slot = 1 / 240.0;
            }

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var paritcle = PhysicsWorld.CreateParticle
                (
                    new Vector2D(x, y),
                    new Vector2D(rnd.NextDouble() * 6 - 3, rnd.NextDouble() * 6 - 3),
                    1f
                );
                _objList.Add(paritcle);
                _contact.AddBall(paritcle, 4.ToSimUnits());
            }
        }

        private readonly ParticleEdge _contact = new ParticleEdge(0.2,
                                                        100.ToSimUnits(),
                                                        350.ToSimUnits(),
                                                        400.ToSimUnits(),
                                                        200.ToSimUnits());
    }
}