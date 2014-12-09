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
using Physics2D.Factories;

using WPFDemo.Graphic;
using WPFDemo.ContactDemo;

namespace WPFDemo.ContactDemo
{
    class ContactDemo : PhysicsGraphic
    {
        private Ball bA;
        private Ball bB;
        private Ball bC;
        private Ball bD;

        public ContactDemo(Image image)
            : base(image)
        {
            // 初始化两个球
            Particle pA = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(20f, 200f)), new Vector2D(2f, 0f), 1f);
            Particle pB = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(200f, 200f)), new Vector2D(0f, 0f), 1f);
            Particle pC = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(240f, 200f)), new Vector2D(0f, 0f), 1f);
            Particle pD = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(280f, 200f)), new Vector2D(0f, 0f), 1f);
            bA = new Ball
            {
                particle = pA,
                r = 20
            };
            bB = new Ball
            {
                particle = pB,
                r = 20
            };
            bC = new Ball
            {
                particle = pC,
                r = 20
            };
            bD = new Ball
            {
                particle = pD,
                r = 20
            };
            var contact = new ParticleBallContact(1f);
            contact.AddBall(bA.particle, ConvertUnits.ToSimUnits(bA.r));
            contact.AddBall(bB.particle, ConvertUnits.ToSimUnits(bB.r));
            contact.AddBall(bC.particle, ConvertUnits.ToSimUnits(bC.r));
            contact.AddBall(bD.particle, ConvertUnits.ToSimUnits(bD.r));

            physicsWorld.RegistryContactGenerator(contact);

            // 将两个球加入到绘制队列
            drawQueue.Add(bA);
            drawQueue.Add(bB);
            drawQueue.Add(bC);
            drawQueue.Add(bD);

            slot = 1 / 60f;

            Start = true;
        }


        protected override void UpdatePhysics(float duration)
        {
            physicsWorld.Update(duration);
        }
    }
}
