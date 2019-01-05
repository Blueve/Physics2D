namespace UnitTest.Object
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Object;

    [TestClass]
    public class ParticleTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            Particle obj = new Particle
            {
                Mass = 1,
                Position = new Vector2D(0, 50)
            };

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestSetMass()
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
                obj.Mass = 0;
            }
            catch (ArgumentOutOfRangeException) { }
            catch (Exception)
            {
                Assert.Fail();
            }

            obj.InverseMass = 0;
            Assert.AreEqual(0, obj.InverseMass);
            Assert.AreEqual(double.MaxValue, obj.Mass);
        }

        [TestMethod]
        public void TestUpdate()
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

        [TestMethod]
        public void TestBindShape()
        {
            Particle obj = new Particle
            {
                Position = new Vector2D(0, 50)
            };
            obj.BindShape(new Circle(10));

            Assert.IsNotNull(obj.Shape as Circle);
            Assert.IsTrue(obj.Shape.Body == obj);
        }
    }
}