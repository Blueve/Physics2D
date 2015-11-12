using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Collision.Shapes;

namespace UnitTest.Collision.Shapes
{
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
