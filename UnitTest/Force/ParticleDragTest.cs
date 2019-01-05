namespace UnitTest.Force
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Common;
    using Physics2D.Force;
    using Physics2D.Object;

    [TestClass]
    public class ParticleDragTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var force = new ParticleDrag(0.1, 0.1);
            Assert.IsNotNull(force);
        }

        [TestMethod]
        public void TestApplyTo()
        {
            var force = new ParticleDrag(0.1, 0.1);
            var particle = new Particle
            {
                Mass = 1
            };
            force.Add(particle);
            force.Apply(1);

            particle.Update(1);
            Assert.AreEqual(new Vector2D(0, 0), particle.Acceleration, "速度为0的物体不受阻力影响");

            particle.Velocity = new Vector2D(1, 0);

            force.Apply(1);
            particle.Update(1);
            Assert.AreEqual(new Vector2D(-0.2, 0), particle.Acceleration, "速度不为0的物体按照公式计算阻力大小");
        }
    }
}
