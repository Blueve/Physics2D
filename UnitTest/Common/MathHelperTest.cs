using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Physics2D.Common;
using System.Collections.Generic;
using Physics2D.Common.Exceptions;

namespace UnitTest.Common
{
    [TestClass]
    public class MathHelperTest
    {
        [TestMethod]
        public void TestPointToLineVector()
        {
            Vector2D linePA = new Vector2D(0, 0);
            Vector2D linePB = new Vector2D(5, 0);
            Vector2D point = new Vector2D(2.5, 5);

            Assert.AreEqual(new Vector2D(0, -5), MathHelper.PointToLineVector(point, linePA, linePB));
        }

        [TestMethod]
        public void TestLineIntersection()
        {
            // 有交点的情况
            Vector2D pA1 = new Vector2D(0, 3);
            Vector2D pA2 = new Vector2D(6, 3);

            Vector2D pB1 = new Vector2D(3, 0);
            Vector2D pB2 = new Vector2D(3, 6);

            var actual = MathHelper.LineIntersection(pA1, pA2, pB1, pB2);
            Assert.AreEqual(new Vector2D(3, 3), actual);

            // 无交点的情况
            Vector2D pC1 = new Vector2D(1, 4);
            Vector2D pC2 = new Vector2D(7, 4);
            Assert.IsNull(MathHelper.LineIntersection(pA1, pA2, pC1, pC2));

            Vector2D pD1 = new Vector2D(12, 6);
            Vector2D pD2 = new Vector2D(12, 0);
            Assert.IsNull(MathHelper.LineIntersection(pA2, pA1, pD1, pD2));
        }

        [TestMethod]
        public void TestIsInside()
        {
            // 测试正常情况
            var vertexs = new List<Vector2D>
            {
                new Vector2D(0, 0),
                new Vector2D(100, 0),
                new Vector2D(0, 100)
            };
            vertexs.Add(vertexs[0]);

            Assert.IsTrue(MathHelper.IsInside(vertexs, new Vector2D(25, 25)));
            Assert.IsFalse(MathHelper.IsInside(vertexs, new Vector2D(100, 100)));
        }

        [TestMethod]
        public void TestIsInsideFail()
        {
            // 测试不能围成多边形的情况
            var vertexs = new List<Vector2D>
            {
                new Vector2D(0, 0),
                new Vector2D(100, 0),
                new Vector2D(0, 100)
            };

            Assert.IsFalse(MathHelper.IsInside(vertexs, new Vector2D(25, 25)));
        }

        [TestMethod]
        public void TestPointToLineDistanceSquared()
        {
            Assert.AreEqual(25, MathHelper.PointToLineDistenceSquared(
                new Vector2D(0, 5),
                new Vector2D(-5, 0), new Vector2D(5, 0)));
        }
    }
}
