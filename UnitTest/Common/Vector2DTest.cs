using Microsoft.VisualStudio.TestTools.UnitTesting;

using Physics2D.Common;

namespace UnitTest.Common
{
    [TestClass]
    public class Vector2DTest
    {
        [TestMethod]
        public void TestVectorConstructor()
        {
            Vector2D vector = new Vector2D();
            vector.X = vector.Y = 0;

            Assert.AreEqual(vector, new Vector2D(0, 0));
            Assert.AreEqual(vector, new Vector2D(vector));
        }

        [TestMethod]
        public void TestVectorOverrideFunction()
        {
            Vector2D vector = new Vector2D(1, 2);

            // Test Equals
            Assert.IsTrue(vector.Equals(new Vector2D(1, 2)));

            // Test ToString
            Assert.AreEqual("(1.23, 4.56)", new Vector2D(1.23, 4.56).ToString());
        }

        [TestMethod]
        public void TestVectorOperator()
        {
            Vector2D left = new Vector2D(3, 4);
            Vector2D right = new Vector2D(2, 2);

            // Test !=
            Assert.IsTrue(left != right);

            Vector2D actual = left + right;
            Vector2D expected = new Vector2D(5, 6);

            // Test ==
            Assert.IsTrue(actual == expected);

            // Test +
            Assert.AreEqual(expected, actual);

            // Test -
            actual = left - right;
            expected = new Vector2D(1, 2);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(new Vector2D(-3, -4), -left);

            // Test *
            Assert.AreEqual(3 * 2 + 4 * 2, left * right);
            Assert.AreEqual(new Vector2D(6, 8), left * 2);
            Assert.AreEqual(new Vector2D(-10, -10), -5 * right);
        }

        [TestMethod]
        public void TestVectorMethod()
        {
            Vector2D vect1 = new Vector2D(3, 0);
            Vector2D vect2 = new Vector2D(0, 4);

            // Test Distance
            Assert.AreEqual(25f, Vector2D.DistanceSquared(vect1, vect2));
            Assert.AreEqual(5f, Vector2D.Distance(vect1, vect2));

            // Test Length
            Assert.AreEqual(3f, vect1.Length());
            Assert.AreEqual(16f, vect2.LengthSquared());

            // Test Normalize
            Assert.AreEqual(Vector2D.UnitX, Vector2D.Normalize(vect1));
            Assert.AreEqual(Vector2D.UnitY, vect2.Normalize());
        }
    }
}