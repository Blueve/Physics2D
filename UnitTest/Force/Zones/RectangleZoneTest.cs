using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Object;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Common;

namespace UnitTest.Force.Zones
{
    [TestClass]
    public class RectangleZoneTest
    {
        [TestMethod]
        public void TestTryApplyTo()
        {
            var pIn = new Particle
            {
                Position = new Vector2D(2, 3),
                Mass = 1
            };
            var pOut = new Particle
            {
                Position = new Vector2D(6, 6),
                Mass = 1
            };
            var zone = new RectangleZone(0, 0, 5, 5);
            zone.Add(new ParticleConstantForce(new Vector2D(5, 0)));

            zone.TryApplyTo(pIn, 1 / 60.0);
            zone.TryApplyTo(pOut, 1 / 60.0);
            pIn.Update(1 / 60.0);
            pOut.Update(1 / 60.0);

            Assert.AreEqual(new Vector2D(5, 0), pIn.Acceleration);
            Assert.AreEqual(new Vector2D(0, 0), pOut.Acceleration);
        }
    }
}
