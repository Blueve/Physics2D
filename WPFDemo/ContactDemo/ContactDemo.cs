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

namespace WPFDemo.ContactDemo
{
    class ContactDemo : PhysicsGraphic
    {

        private List<Ball> ballList = new List<Ball>();

        public ContactDemo(Image image)
            : base(image)
        {
            int num = 5;

            var contact = new ParticleBall(1f);
            physicsWorld.RegistryContactGenerator(contact);

            for(int i = 0; i < num; i++)
            {
                Particle fB = physicsWorld.CreateFixedParticle((new Vector2D(160 + 40 * i, 0f)).ToSimUnits());
                Particle pB = physicsWorld.CreateParticle((new Vector2D(160 + 40 * i, 200f)).ToSimUnits(), new Vector2D(0, 0), 2);
                Ball ball = new Ball
                {
                    fixedParticle = fB,
                    particle = pB,
                    r = 20
                };
                physicsWorld.RegistryContactGenerator(new ParticleRope(ConvertUnits.ToSimUnits(200), 0f, fB, pB));
                contact.AddBall(ball.particle, ConvertUnits.ToSimUnits(ball.r));
                drawQueue.Add(ball);
                ballList.Add(ball);
            }

            // 增加重力和空气阻力
            physicsWorld.CreateGlobalZone(new ParticleGravity(new Vector2D(0f, 40f)));

            slot = 1 / 240.0;

            Start = true;
        }


        protected override void UpdatePhysics(double duration)
        {
            physicsWorld.Update(duration);
        }

        public void Fire()
        {
            ballList[0].particle.Velocity = new Vector2D(-10f, 0f);
        }
    }
}
