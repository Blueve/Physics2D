using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Core;
using Physics2D.Force;
using Physics2D.Object;
using Physics2D.Common;

namespace UnitTest.Core
{
    [TestClass]
    public class ForceRegistryTest
    {
        [TestMethod]
        public void TestUpdate()
        {
            var forceRegistry = new ForceRegistry();
            var force = new ParticleConstantForce(new Vector2D(5, 0));
            var p = new Particle { Mass = 1 };
            force.Add(p);

            TestAddForceGenerator(forceRegistry, force);
            forceRegistry.Update(1 / 60.0);
            p.Update(1 / 60.0);
            Assert.AreEqual(new Vector2D(5, 0), p.Acceleration, "物体被赋予正确的加速度");

            p.Acceleration = Vector2D.Zero;
            TestRemoveForceGenerator(forceRegistry, force);
            forceRegistry.Update(1 / 60.0);
            p.Update(1 / 60.0);
            Assert.AreEqual(new Vector2D(0, 0), p.Acceleration, "删去作用力发生器，物体不再受该作用力发生器所产生的力");

            force.Add(p);
            TestAddForceGenerator(forceRegistry, force);
            TestRemoveParticle(forceRegistry, p);
            forceRegistry.Update(1 / 60.0);
            p.Update(1 / 60.0);
            Assert.AreEqual(new Vector2D(0, 0), p.Acceleration, "删去物体，物体不再受该作用力发生器所产生的力");
        }

        private void TestAddForceGenerator(
            ForceRegistry forceRegistry, 
            ParticleForceGenerator force)
        {
            forceRegistry.Add(force);
        }

        private void TestRemoveForceGenerator(
            ForceRegistry forceRegistry,
            ParticleForceGenerator force)
        {
            forceRegistry.Remove(force);
        }

        private void TestRemoveParticle(
            ForceRegistry forceRegistry,
            Particle p)
        {
            forceRegistry.Remove(p);
        }
        
    }
}
