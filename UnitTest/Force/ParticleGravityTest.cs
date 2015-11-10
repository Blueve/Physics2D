using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Force;
using Physics2D.Common;
using Physics2D.Object;

namespace UnitTest.Force
{
    [TestClass]
    public class ParticleGravityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var force = new ParticleGravity(new Vector2D(0, 9.8));
            Assert.IsNotNull(force);
        }

        [TestMethod]
        public void TestApplyTo()
        {
            var force = new ParticleGravity(new Vector2D(0, 9.8));
            var particle = new Particle
            {
                Mass = 2
            };
            force.Add(particle);
            force.Apply(1);
            particle.Update(1);
            Assert.AreEqual(new Vector2D(0, 9.8), particle.Acceleration, "根据公式计算物体所应受到的重力");

            particle.InverseMass = 0;
            force.Apply(1);
            particle.Update(1);
            Assert.AreEqual(new Vector2D(0, 0), particle.Acceleration, "质量无限大的物体设定为不受重力影响");

            force.Remove(particle);
            force.Apply(1);
            particle.Update(1);
            Assert.AreEqual(new Vector2D(0, 0), particle.Acceleration, "被从作用力发生器中移除的物体不受重力影响");
        }
    }
}
