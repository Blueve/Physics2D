namespace WPFDemo.ContactDemo
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Physics2D;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Force;
    using WPFDemo.Graphic;

    internal class ContactDemo : PhysicsGraphic
    {
        /// <summary>
        /// 钢珠列表
        /// </summary>
        private readonly List<Ball> ballList = new List<Ball>();

        public ContactDemo(Image image)
            : base(image)
        {
            Settings.ContactIteration = 1;

            const int num = 5;

            for (int i = 0; i < num; i++)
            {
                var fB = this.PhysicsWorld.CreateFixedParticle(new Vector2D(160 + 40 * i, 0).ToSimUnits());
                var pB = this.PhysicsWorld.CreateParticle(new Vector2D(160 + 40 * i, 200).ToSimUnits(), new Vector2D(0, 0), 2);

                var ball = new Ball
                {
                    FixedParticle = fB,
                    Particle = pB,
                    R = 20
                };

                // 为质体绑定形状
                ball.Particle.BindShape(new Circle(ball.R.ToSimUnits()));

                this.PhysicsWorld.CreateRope(200.ToSimUnits(), 0, fB, pB);
                this.DrawQueue.Add(ball);
                this.ballList.Add(ball);
            }

            // 增加重力和空气阻力
            this.PhysicsWorld.CreateGlobalZone(new ParticleGravity(new Vector2D(0, 40)));
            this.PhysicsWorld.CreateParticle(Vector2D.Zero, new Vector2D(1, 0), 1);
            this.Slot = 1 / 120.0;

            this.Start = true;
        }

        protected override void UpdatePhysics(double duration)
        {
            this.PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            this.ballList[0].Particle.Velocity = new Vector2D(-10, 0);
        }
    }
}
