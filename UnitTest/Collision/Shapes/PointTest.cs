namespace UnitTest.Collision.Shapes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Collision.Shapes;

    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void TestType()
        {
            var p = new Point();

            Assert.AreEqual(ShapeType.Point, p.Type);
            Assert.AreEqual(0, p.Id);
        }
    }
}
