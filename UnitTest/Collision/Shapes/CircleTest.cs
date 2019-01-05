namespace UnitTest.Collision.Shapes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Collision.Shapes;

    [TestClass]
    public class CircleTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var circle = new Circle(10);
            Assert.AreEqual(10, circle.R);
            Assert.AreEqual(0, circle.Id);

            circle = new Circle(10, 1);
            Assert.AreEqual(1, circle.Id);
        }

        [TestMethod]
        public void TestType()
        {
            var circle = new Circle(10);
            Assert.AreEqual(ShapeType.Circle, circle.Type);
        }
    }
}
