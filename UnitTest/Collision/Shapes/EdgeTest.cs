using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Common;
using Physics2D.Collision.Shapes;

namespace UnitTest.Collision.Shapes
{
    [TestClass]
    public class EdgeTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var edge = new Edge(new Vector2D(0, 0), new Vector2D(1, 1));
            Assert.AreEqual(new Vector2D(0, 0), edge.PointA);
            Assert.AreEqual(new Vector2D(1, 1), edge.PointB);

            edge = new Edge(0, 0, 1, 1);
            Assert.AreEqual(new Vector2D(0, 0), edge.PointA);
            Assert.AreEqual(new Vector2D(1, 1), edge.PointB);
            Assert.AreEqual(0, edge.Id);

        }

        [TestMethod]
        public void TestType()
        {
            var edge = new Edge(new Vector2D(0, 0), new Vector2D(1, 1));

            Assert.AreEqual(ShapeType.Edge, edge.Type);
        }
    }
}
