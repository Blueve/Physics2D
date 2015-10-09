using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Physics2D.Common;

namespace UnitTest.Common
{
    [TestClass]
    public class MathHelperTest
    {
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
        }
    }
}
