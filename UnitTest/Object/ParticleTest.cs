using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Common;
using Physics2D.Object;
using System;

namespace UnitTest.Object
{
    [TestClass]
    public class ParticleTest
    {
        [TestMethod]
        public void TestParticleConstructor()
        {
            Particle obj = new Particle
            {
                Mass = 1f,
                Position = new Vector2D(0f, 50f)
            };

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestParticleSetMass()
        {
            Particle obj = new Particle
            {
                Position = new Vector2D(0f, 50f)
            };
            // Normal
            obj.Mass = 2f;
            Assert.AreEqual(2f, obj.Mass);
            Assert.AreEqual(0.5f, obj.InverseMass);

            obj.InverseMass = 2f;
            Assert.AreEqual(0.5f, obj.Mass);
            Assert.AreEqual(2f, obj.InverseMass);

            // Zero
            try
            {
                obj.Mass = 0f;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
            catch (Exception)
            {
                throw new AssertFailedException();
            }

            obj.InverseMass = 0f;
            Assert.AreEqual(0f, obj.InverseMass);
            Assert.AreEqual(float.MaxValue, obj.Mass);
        }

        [TestMethod]
        public void TestParticleUpdate()
        {
            Particle obj = new Particle
            {
                Mass = 1f,
                Position = new Vector2D(0f, 0f)
            };
            obj.AddForce(new Vector2D(1f, 0f));

            obj.Update(1f);
            Assert.AreEqual(new Vector2D(0f, 0f), obj.Position);
            Assert.AreEqual(new Vector2D(1f, 0f), obj.Velocity);
            Assert.AreEqual(new Vector2D(0f, 0f), obj.Acceleration);

            obj.Update(1f);
            obj.Update(1f);
            Assert.AreEqual(new Vector2D(2f, 0f), obj.Position);
            Assert.AreEqual(new Vector2D(1f, 0f), obj.Velocity);
            Assert.AreEqual(new Vector2D(0f, 0f), obj.Acceleration);
        }
    }
}