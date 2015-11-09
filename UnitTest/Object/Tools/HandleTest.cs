using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Object.Tools;
using Physics2D.Common;

namespace UnitTest.Object.Tools
{
    [TestClass]
    public class HandleTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            Handle handle = new Handle(new Vector2D(0, 0));

            Assert.IsNotNull(handle);
            Assert.AreEqual(new Vector2D(0, 0), handle.Position);
        }

        [TestMethod]
        public void TestPropertyChangedEvent()
        {
            Handle handle = new Handle(new Vector2D(0, 0));

            Vector2D position = Vector2D.Zero;
            handle.PropertyChanged += (sender, e) =>
            {
                position = (sender as Handle).Position;
            };

            handle.Position = new Vector2D(1, 0);
            Assert.AreEqual(new Vector2D(1, 0), position);

            handle.Position = new Vector2D(1, 0);
            Assert.AreEqual(new Vector2D(1, 0), position);
        }

        [TestMethod]
        public void TestRelease()
        {
            Handle handle = new Handle(new Vector2D(0, 0));

            Vector2D position = Vector2D.Zero;
            handle.PropertyChanged += (sender, e) =>
            {
                position = (sender as Handle).Position;
            };
            handle.Release();

            handle.Position = new Vector2D(1, 0);
            Assert.AreEqual(Vector2D.Zero, position);
        }
    }
}
