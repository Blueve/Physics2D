using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Force;
using Physics2D.Common;
using Physics2D.Object;

namespace UnitTest.Force
{
    [TestClass]
    public class ParticleConstantForceTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var force = new ParticleConstantForce(new Vector2D(5, 0));
            Assert.IsNotNull(force);
        }

        [TestMethod]
        public void TestApplyTo()
        {
            var force = new ParticleConstantForce(new Vector2D(5, 0));
            var particle = new Particle
            {
                Mass = 1
            };
            force.Add(particle);
            force.Apply(1);

            particle.Update(1);

            Assert.AreEqual(new Vector2D(5, 0), particle.Acceleration);
        }
    }
}
