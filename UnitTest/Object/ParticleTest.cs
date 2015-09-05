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
                Mass = 1,
                Position = new Vector2D(0, 50)
            };

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestParticleSetMass()
        {
            Particle obj = new Particle
            {
                Position = new Vector2D(0, 50)
            };
            // Normal
            obj.Mass = 2;
            Assert.AreEqual(2, obj.Mass);
            Assert.AreEqual(0.5, obj.InverseMass);

            obj.InverseMass = 2;
            Assert.AreEqual(0.5, obj.Mass);
            Assert.AreEqual(2, obj.InverseMass);

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

            obj.InverseMass = 0;
            Assert.AreEqual(0, obj.InverseMass);
            Assert.AreEqual(double.MaxValue, obj.Mass);
        }

        [TestMethod]
        public void TestParticleUpdate()
        {
            Particle obj = new Particle
            {
                Mass = 1,
                Position = new Vector2D(0, 0)
            };
            obj.AddForce(new Vector2D(1, 0));

            obj.Update(1);
            Assert.AreEqual(new Vector2D(0, 0), obj.Position);
            Assert.AreEqual(new Vector2D(1, 0), obj.Velocity);
            Assert.AreEqual(new Vector2D(1, 0), obj.Acceleration);

            obj.Update(1);
            obj.Update(1);
            Assert.AreEqual(new Vector2D(2, 0), obj.Position);
            Assert.AreEqual(new Vector2D(1, 0), obj.Velocity);
            Assert.AreEqual(new Vector2D(0, 0), obj.Acceleration);
        }
    }
}