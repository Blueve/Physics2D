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
        private Ball bA;
        private Ball bB;
        private Ball bC;
        private Ball bD;

        public ContactDemo(Image image)
            : base(image)
        {
            // 初始化四个悬挂球
            Particle fA = ParticleFactory.CreateFixed(physicsWorld, ConvertUnits.ToSimUnits(new Vector2D(180f, 0f)));
            Particle fB = ParticleFactory.CreateFixed(physicsWorld, ConvertUnits.ToSimUnits(new Vector2D(220f, 0f)));
            Particle fC = ParticleFactory.CreateFixed(physicsWorld, ConvertUnits.ToSimUnits(new Vector2D(260f, 0f)));
            Particle fD = ParticleFactory.CreateFixed(physicsWorld, ConvertUnits.ToSimUnits(new Vector2D(300f, 0f)));

            Particle pA = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(50f, 50f)), new Vector2D(0f, 0f), 1f);
            Particle pB = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(220f, 250f)), new Vector2D(0f, 0f), 1f);
            Particle pC = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(260f, 250f)), new Vector2D(0f, 0f), 1f);
            Particle pD = physicsWorld.CreateParticle(ConvertUnits.ToSimUnits(new Vector2D(300f, 250f)), new Vector2D(0f, 0f), 1f);

            bA = new Ball
            {
                fixedParticle = fA,
                particle = pA,
                r = 20
            };
            bB = new Ball
            {
                fixedParticle = fB,
                particle = pB,
                r = 20
            };
            bC = new Ball
            {
                fixedParticle = fC,
                particle = pC,
                r = 20
            };
            bD = new Ball
            {
                fixedParticle = fD,
                particle = pD,
                r = 20
            };

            // 将四个球分别和固定点连接
            physicsWorld.RegistryContactGenerator(new ParticleRope(ConvertUnits.ToSimUnits(250f), 0f, fA, pA));
            physicsWorld.RegistryContactGenerator(new ParticleRope(ConvertUnits.ToSimUnits(250f), 0f, fB, pB));
            physicsWorld.RegistryContactGenerator(new ParticleRope(ConvertUnits.ToSimUnits(250f), 0f, fC, pC));
            physicsWorld.RegistryContactGenerator(new ParticleRope(ConvertUnits.ToSimUnits(250f), 0f, fD, pD));

            // 将四个球加入碰撞检测
            var contact = new ParticleBall(0.99f);
            contact.AddBall(bA.particle, ConvertUnits.ToSimUnits(bA.r));
            contact.AddBall(bB.particle, ConvertUnits.ToSimUnits(bB.r));
            contact.AddBall(bC.particle, ConvertUnits.ToSimUnits(bC.r));
            contact.AddBall(bD.particle, ConvertUnits.ToSimUnits(bD.r));

            physicsWorld.RegistryContactGenerator(contact);

            // 增加重力和空气阻力
            ZoneFactory.CreateGloablZone(physicsWorld, new ParticleGravity(new Vector2D(0f, 10f)));
            ZoneFactory.CreateGloablZone(physicsWorld, new ParticleDrag(0.02f, 0.01f));

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

        public void Fire()
        {
            bA.particle.Velocity = new Vector2D(-3f, 0f);
        }
    }
}
