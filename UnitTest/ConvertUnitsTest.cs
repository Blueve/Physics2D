using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Common;
using Physics2D;


namespace UnitTest
{
    [TestClass]
    public class ConvertUnitsTest
    {
        [TestMethod]
        public void TestToSimUnits()
        {
            Assert.AreEqual(2, 100.ToSimUnits());
            Assert.AreEqual(2, (100.0).ToSimUnits());
            Assert.AreEqual(new Vector2D(2, 1), (new Vector2D(100, 50)).ToSimUnits());
        }

        [TestMethod]
        public void TestToDisplayUnits()
        {
            Assert.AreEqual(100, 2.ToDisplayUnits());
            Assert.AreEqual(100, (2.0).ToDisplayUnits());
            Assert.AreEqual(new Vector2D(100, 50), (new Vector2D(2, 1)).ToDisplayUnits());
        }

        [TestMethod]
        public void TestSetDisplayUnitToSimUnitRatio()
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(50);

            TestToSimUnits();
            TestToDisplayUnits();
        }
    }
}
