namespace UnitTest.Factories
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Common;
    using Physics2D.Core;
    using Physics2D.Factories;
    using Physics2D.Object;

    [TestClass]
    public class ParticleFactoryTest
    {
        [TestMethod]
        public void TestCreateParticle()
        {
            var world = new World();

            var p = world.CreateParticle(new Vector2D(0, 1), new Vector2D(1, 0), 1, 1);
            this.TestParticleProperty(p, new Vector2D(0, 1), new Vector2D(1, 0), 1, 1);
        }

        [TestMethod]
        public void TestCreateFixedParticle()
        {
            var world = new World();

            var p = world.CreateFixedParticle(new Vector2D(0, 1));
            this.TestParticleProperty(p, new Vector2D(0, 1), Vector2D.Zero, double.MaxValue, 1);
        }

        [TestMethod]
        public void TestCreateUnstoppableParticle()
        {
            var world = new World();

            var p = world.CreateUnstoppableParticle(new Vector2D(0, 1), new Vector2D(1, 0));
            this.TestParticleProperty(p, new Vector2D(0, 1), new Vector2D(1, 0), double.MaxValue, 1);
        }

        private void TestParticleProperty(
            Particle particle,
            Vector2D p,
            Vector2D v,
            double m,
            double restitution)
        {
            Assert.AreEqual(p, particle.Position, "位置");
            Assert.AreEqual(v, particle.Velocity, "速度");
            Assert.AreEqual(m, particle.Mass, "质量");
            Assert.AreEqual(restitution, particle.Restitution, "回弹系数");
        }
    }
}
