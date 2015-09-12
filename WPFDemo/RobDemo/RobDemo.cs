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
        private Particle _a = new Particle
        {
            Position = (new Vector2D(200, 0)).ToSimUnits(),
            InverseMass = 0
        };
        private Particle _b = new Particle
        {
            Position = (new Vector2D(300, 20)).ToSimUnits(),
            Mass = 1
        };
        private Particle _c = new Particle
        {
            Position = (new Vector2D(300, 80)).ToSimUnits(),
            Mass = 1
        };
        #endregion

        #region 底边
        private readonly ParticleEdge _contact = new ParticleEdge(0.5,
                                                        0.ToSimUnits(),
                                                        390.ToSimUnits(),
                                                        500.ToSimUnits(),
                                                        390.ToSimUnits());
        #endregion

        public RobDemo(Image image)
            : base(image)
        {
            PhysicsWorld += _a;
            PhysicsWorld += _b;
            PhysicsWorld += _c;

            _contact.AddBall(_a, 4.ToSimUnits());
            _contact.AddBall(_b, 4.ToSimUnits());
            _contact.AddBall(_c, 4.ToSimUnits());

            // 连接三个顶点
            //PhysicsWorld.RegistryContactGenerator(new ParticleRod(_a, _b));
            //PhysicsWorld.RegistryContactGenerator(new ParticleRod(_b, _c));
            PhysicsWorld.RegistryContactGenerator(new ParticleRod(_a, _c));

            // 增加底部边缘
            PhysicsWorld.RegistryContactGenerator(_contact);

            DrawQueue.Add(this);
        }

        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            if (!Start)
            {
                Start = true;
                // 增加重力
                PhysicsWorld.CreateGlobalZone(new ParticleGravity(new Vector2D(0, 10)));
            }
        }

        public void Draw(WriteableBitmap bitmap)
        {
            //bitmap.DrawLineAa(
            //    _a.Position.X.ToDisplayUnits(), _a.Position.Y.ToDisplayUnits(),
            //    _b.Position.X.ToDisplayUnits(), _b.Position.Y.ToDisplayUnits(), Colors.Black);
            //bitmap.DrawLineAa(
            //    _c.Position.X.ToDisplayUnits(), _c.Position.Y.ToDisplayUnits(),
            //    _b.Position.X.ToDisplayUnits(), _b.Position.Y.ToDisplayUnits(), Colors.Black);
            bitmap.DrawLineAa(
                _a.Position.X.ToDisplayUnits(), _a.Position.Y.ToDisplayUnits(),
                _c.Position.X.ToDisplayUnits(), _c.Position.Y.ToDisplayUnits(), Colors.Black);

            bitmap.DrawLineAa(0, 390, 500, 390, Colors.Black);
        }
    }
}
