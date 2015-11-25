using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Collision.Basic;
using Physics2D.Object;
using Physics2D.Common;

namespace UnitTest.Collision.Basic
{
    [TestClass]
    public class ParticleRodTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var pA = new Particle { Mass = 1 };
            var pB = new Particle { Mass = 1, Position = new Vector2D(5, 0) };
            var rod = new ParticleRod(pA, pB);
            
            Assert.AreEqual(pA, rod.ParticleA);
            Assert.AreEqual(pB, rod.ParticleB);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            var pA = new Particle { Mass = 1 };
            var pB = new Particle { Mass = 1, Position = new Vector2D(5, 0) };
            var rod = new ParticleRod(pA, pB);

            foreach (var contact in rod)
            {
                Assert.Fail("长度未变化时不产生任何碰撞");
            }

            pB.Position = new Vector2D(6, 0);
            foreach (var contact in rod)
            {
                Assert.AreEqual(1, contact.Penetration);
                Assert.AreEqual(new Vector2D(1, 0), contact.ContactNormal);
            }

            pB.Position = new Vector2D(4, 0);
            foreach (var contact in rod)
            {
                Assert.AreEqual(1, contact.Penetration);
                Assert.AreEqual(new Vector2D(-1, 0), contact.ContactNormal);
            }
        }
    }
}
