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
            Vector2D pA1 = new Vector2D(0, 3);
            Vector2D pA2 = new Vector2D(6, 3);

            Vector2D pB1 = new Vector2D(3f, 0);
            Vector2D pB2 = new Vector2D(3, 6);

            var actual = MathHelper.LineIntersection(pA1, pA2, pB1, pB2);
            Assert.AreEqual(new Vector2D(3, 3), actual);
        }
    }
}
