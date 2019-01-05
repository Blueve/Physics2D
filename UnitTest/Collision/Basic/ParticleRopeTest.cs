namespace UnitTest.Collision.Basic
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Collision;
    using Physics2D.Collision.Basic;
    using Physics2D.Common;
    using Physics2D.Object;

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
            Assert.AreEqual(pA, rope.ParticleA);
            Assert.AreEqual(pB, rope.ParticleB);
            Assert.AreEqual(0.5, rope.Restitution);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            var pA = new Particle { Mass = 1 };
            var pB = new Particle { Mass = 1 };
            var rope = new ParticleRope(10, 0.5, pA, pB);

            var contacts = new List<ParticleContact>();
            contacts.AddRange(rope);
            Assert.AreEqual(0, contacts.Count, "长度未变化时不产生任何碰撞");

            pB.Position = new Vector2D(20, 0);
            foreach (var contact in rope)
            {
                Assert.AreEqual(10, contact.Penetration);
                Assert.AreEqual(new Vector2D(1, 0), contact.ContactNormal);
            }
        }
    }
}
