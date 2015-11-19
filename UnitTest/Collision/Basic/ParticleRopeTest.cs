using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Collision.Basic;
using Physics2D.Object;
using Physics2D.Common;

namespace UnitTest.Collision.Basic
{
    [TestClass]
    public class ParticleRopeTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var pA = new Particle { Mass = 1 };
            var pB = new Particle { Mass = 1 };
            var rope = new ParticleRope(10, 0.5, pA, pB);

            Assert.AreEqual(10, rope.MaxLength);
            Assert.AreEqual(pA, rope.PA);
            Assert.AreEqual(pB, rope.PB);
            Assert.AreEqual(0.5, rope.Restitution);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            var pA = new Particle { Mass = 1 };
            var pB = new Particle { Mass = 1 };
            var rope = new ParticleRope(10, 0.5, pA, pB);

            foreach (var contact in rope)
            {
                Assert.Fail("未超过绳长时不产生任何碰撞");
            }

            pB.Position = new Vector2D(20, 0);
            foreach (var contact in rope)
            {
                Assert.AreEqual(10, contact.Penetration);
                Assert.AreEqual(new Vector2D(1, 0), contact.ContactNormal);
            }
        }
    }
}
