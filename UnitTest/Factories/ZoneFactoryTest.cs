namespace UnitTest.Factories
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Common;
    using Physics2D.Core;
    using Physics2D.Factories;
    using Physics2D.Force;

    [TestClass]
    public class ZoneFactoryTest
    {
        [TestMethod]
        public void TestCreateGlobalZone()
        {
            var world = new World();
            var zone = world.CreateGlobalZone(new ParticleConstantForce(new Vector2D(0, 5)));
            Assert.IsNotNull(zone);
        }

        [TestMethod]
        public void TestCreateRectangleZone()
        {
            var world = new World();
            var zone = world.CreateRectangleZone(
                new ParticleConstantForce(new Vector2D(0, 5)),
                1, 2, 3, 4);
            Assert.IsNotNull(zone);
            Assert.AreEqual(zone.X1, 1);
            Assert.AreEqual(zone.Y1, 2);
            Assert.AreEqual(zone.X2, 3);
            Assert.AreEqual(zone.Y2, 4);
        }

        [TestMethod]
        public void TestCreateGravity()
        {
            var world = new World();
            var zone = world.CreateGravity(9.8);
            Assert.IsNotNull(zone);
        }
    }
}
