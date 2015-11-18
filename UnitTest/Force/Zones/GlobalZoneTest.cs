using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Force.Zones;
using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Object;

namespace UnitTest.Force.Zones
{
    [TestClass]
    public class GlobalZoneTest
    {
        [TestMethod]
        public void TestTryApplyTo()
        {
            var p = new Particle
            {
                Mass = 1
            };
            var zone = new GlobalZone();
            zone.Add(new ParticleConstantForce(new Vector2D(5, 0)));

            zone.TryApplyTo(p, 1 / 60.0);
            p.Update(1 / 60.0);

            Assert.AreEqual(new Vector2D(5, 0), p.Acceleration);
        }
    }
}
