namespace UnitTest.Force
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Common;
    using Physics2D.Force;
    using Physics2D.Object;

    [TestClass]
    public class ParticleElasticTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var force = new ParticleElastic(1, 2);
            Assert.IsNotNull(force);
        }

        [TestMethod]
        public void TestApplyTo()
        {
            var force = new ParticleElastic(1, 2);
            var pA = new Particle
            {
                Position = new Vector2D(0, 0),
                Mass = 1
            };
            var pB = new Particle
            {
                Position = new Vector2D(1, 0),
                Mass = 1
            };
            force.Add(pA);
            force.LinkWith(pB);

            force.Apply(1);

            pA.Update(1);
            Assert.AreEqual(new Vector2D(-1, 0), pA.Acceleration);
        }
    }
}
