namespace UnitTest.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Common;

    [TestClass]
    public class Vector2DTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            Vector2D vector = new Vector2D();
            vector.X = vector.Y = 0;

            Assert.AreEqual(vector, new Vector2D(0, 0));
            Assert.AreEqual(vector, new Vector2D(vector));
        }

        [TestMethod]
        public void TestEquals()
        {
            Vector2D vector = new Vector2D(1, 2);

            Assert.IsTrue(vector.Equals(new Vector2D(1, 2)));
            Assert.IsFalse(vector.Equals(new Vector2D(2, 1)));
            Assert.IsFalse(vector.Equals("(1, 2)"));
            Assert.IsFalse(vector.Equals(null));
        }

        [TestMethod]
        public void TestSet()
        {
            Vector2D vector = new Vector2D(0, 0);
            vector.Set(5, 2);

            Assert.AreEqual(new Vector2D(5, 2), vector);
        }

        [TestMethod]
        public void TestSpecificProperty()
        {
            Assert.AreEqual(new Vector2D(1, 1), Vector2D.One);
            Assert.AreEqual(new Vector2D(1, 0), Vector2D.UnitX);
            Assert.AreEqual(new Vector2D(0, 1), Vector2D.UnitY);
            Assert.AreEqual(new Vector2D(0, 0), Vector2D.Zero);
        }

        [TestMethod]
        public void TestToString()
        {
            Vector2D vector = new Vector2D(1, 2);

            Assert.AreEqual("(1.23, 4.56)", new Vector2D(1.23, 4.56).ToString());
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            Vector2D vector = new Vector2D(1, 2);

            Assert.IsNotNull(vector.GetHashCode());
        }

        [TestMethod]
        public void TestAddition()
        {
            Vector2D left = new Vector2D(3, 4);
            Vector2D right = new Vector2D(2, 2);

            Assert.AreEqual(new Vector2D(5, 6), left + right);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            Vector2D left = new Vector2D(3, 4);
            Vector2D right = new Vector2D(2, 2);

            Assert.AreEqual(new Vector2D(1, 2), left - right);
            Assert.AreEqual(new Vector2D(-3, -4), -left);
        }

        [TestMethod]
        public void TestMultiply()
        {
            Vector2D left = new Vector2D(3, 4);
            Vector2D right = new Vector2D(2, 2);

            Assert.AreEqual(3 * 2 + 4 * 2, left * right);
            Assert.AreEqual(new Vector2D(6, 8), left * 2);
            Assert.AreEqual(new Vector2D(-10, -10), -5 * right);
        }

        [TestMethod]
        public void TestDivision()
        {
            Vector2D vector = new Vector2D(3, 4);

            Assert.AreEqual(new Vector2D(1.5, 2), vector / 2);
        }

        [TestMethod]
        public void TestUnaryNegation()
        {
            Vector2D vector = new Vector2D(3, 4);

            Assert.AreEqual(new Vector2D(-3, -4), -vector);
        }

        [TestMethod]
        public void TestEquality()
        {
            Vector2D left = new Vector2D(3, 4);
            Vector2D right = new Vector2D(2, 2);

            Assert.IsFalse(left == right);
            Assert.IsTrue(left == new Vector2D(3, 4));
        }

        [TestMethod]
        public void TestInequality()
        {
            Vector2D left = new Vector2D(3, 4);
            Vector2D right = new Vector2D(2, 2);

            Assert.IsTrue(left != right);
            Assert.IsFalse(left != new Vector2D(3, 4));
        }

        [TestMethod]
        public void TestNormalize()
        {
            Vector2D vect1 = new Vector2D(3, 0);
            Vector2D vect2 = new Vector2D(0, 4);

            Assert.AreEqual(Vector2D.UnitX, Vector2D.Normalize(vect1));
            Assert.AreEqual(Vector2D.UnitY, vect2.Normalize());
            Assert.AreEqual(Vector2D.Zero, Vector2D.Zero.Normalize());
        }

        [TestMethod]
        public void TestLength()
        {
            Vector2D vect1 = new Vector2D(3, 0);
            Vector2D vect2 = new Vector2D(0, 4);

            Assert.AreEqual(3f, vect1.Length());
            Assert.AreEqual(16f, vect2.LengthSquared());
        }

        [TestMethod]
        public void TestDistance()
        {
            Vector2D vect1 = new Vector2D(3, 0);
            Vector2D vect2 = new Vector2D(0, 4);

            Assert.AreEqual(25.0, Vector2D.DistanceSquared(vect1, vect2));
            Assert.AreEqual(5.0, Vector2D.Distance(vect1, vect2));
            Assert.AreEqual(0, Vector2D.Distance(vect1, vect1));
        }
    }
}