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

namespace WPFDemo.ContactDemo
{
    class ContactDemo : PhysicsGraphic
    {

        private readonly List<Ball> _ballList = new List<Ball>();

        public ContactDemo(Image image)
            : base(image)
        {
            Settings.ContactIteration = 1;

            const int num = 5;

            var contact = new ParticleBall(1);
            PhysicsWorld.ContactGenerators.Add(contact);

            for(int i = 0; i < num; i++)
            {
                var fB = PhysicsWorld.CreateFixedParticle((new Vector2D(160 + 40 * i, 0)).ToSimUnits());
                var pB = PhysicsWorld.CreateParticle((new Vector2D(160 + 40 * i, 200)).ToSimUnits(), new Vector2D(0, 0), 2);
                var ball = new Ball
                {
                    FixedParticle = fB,
                    Particle = pB,
                    R = 20
                };
                PhysicsWorld.CreateRope(200.ToSimUnits(), 0, fB, pB);
                contact.PayAttentionTo(ball.Particle, ball.R.ToSimUnits());
                DrawQueue.Add(ball);
                _ballList.Add(ball);
            }

            // 增加重力和空气阻力
            PhysicsWorld.CreateGlobalZone(new ParticleGravity(new Vector2D(0f, 40)));
            PhysicsWorld.CreateParticle(Vector2D.Zero, new Vector2D(1, 0), 1);
            Slot = 1 / 1000.0;

            Start = true;
        }


        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            _ballList[0].Particle.Velocity = new Vector2D(-10, 0);
        }
    }
}
