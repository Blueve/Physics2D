using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Object;
using System.Collections.Generic;
using Physics2D.Common;
using Physics2D.Common.Exceptions;
using Physics2D.Object.Tools;
using Physics2D.Core;

namespace UnitTest.Object
{
    [TestClass]
    public class CombinedParticleTest
    {
        [TestMethod]
        public void TestConstructorForCloseObject()
        {
            // 封闭形状
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                });
            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.Vertexs.Count == 4);

            // 无法形成封闭形状物体
            try
            {
                CombinedParticle objB = new CombinedParticle(
                    new List<Vector2D>
                    {
                        new Vector2D(0, 0),
                        new Vector2D(0, 1)
                    });
            }
            catch (InvalidArgumentException) { }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructorForNotCloseObject()
        {
            

            // 不封闭物体
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1),
                    new Vector2D(1, 0),
                }, isClose: false);
            Assert.IsTrue(obj.Vertexs.Count == 4);
        }

        [TestMethod]
        public void TestConstructorFail()
        {
            CombinedParticle obj = null;

            // 无法构成组合物体
            try
            {
                 obj = new CombinedParticle(
                    new List<Vector2D>
                    {
                        new Vector2D(0, 0)
                    }, isClose: false);
            }
            catch (InvalidArgumentException) { }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsNull(obj, "要创建一个不封闭的组合质体，至少需要两个点");

            // 无法构成封闭的组合物体
            try
            {
                obj = new CombinedParticle(
                    new List<Vector2D>
                    {
                        new Vector2D(0, 0),
                        new Vector2D(0, 1)
                    });
            }
            catch (InvalidArgumentException) { }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsNull(obj, "要创建一个封闭的组合质体，至少需要三个点");
        }

        [TestMethod]
        public void TestIsClose()
        {
            CombinedParticle objA = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                });
            Assert.IsTrue(objA.IsClose);

            CombinedParticle objB = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1),
                    new Vector2D(1, 0),
                }, isClose: false);
            Assert.IsFalse(objB.IsClose);
        }

        [TestMethod]
        public void TestGetRods()
        {
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                });
            Assert.IsNotNull(obj.Rods);
            Assert.IsTrue(obj.Rods.Count == 3);
        }

        [TestMethod]
        public void TestIPinWithCloseObject()
        {
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                });

            World world = new World();

            // Pin
            var handle = world.Pin(obj, Vector2D.UnitX);
            Assert.AreEqual(Vector2D.UnitX, handle.Position);

            // 移动handle
            handle.Position = Vector2D.Zero;
            Assert.AreEqual(new Vector2D(-1, 0), obj.Position);
            Assert.IsTrue(obj.PinRods.Count == 3);

            // UnPin
            world.UnPin(obj);
            Assert.IsTrue(obj.PinRods.Count == 0);
        }

        [TestMethod]
        public void TestIPinWithNotCloseObject()
        {
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1),
                    new Vector2D(1, 0)
                }, isClose: false);

            World world = new World();

            // Pin
            var handle = world.Pin(obj, Vector2D.UnitX);
            Assert.AreEqual(Vector2D.UnitX, handle.Position);

            // 移动handle
            handle.Position = Vector2D.Zero;
            Assert.AreEqual(new Vector2D(-1, 0), obj.Position);
            Assert.IsTrue(obj.PinRods.Count == 4);

            // UnPin
            world.UnPin(obj);
            Assert.IsTrue(obj.PinRods.Count == 0);
        }

        [TestMethod]
        public void TestCustomObject()
        {
            World world = new World();
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                });
            try
            {
                obj.OnInit(world);
                obj.OnRemove(world);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestUpdateWithCloseObject()
        {
            // 封闭物体
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                });
            obj.Position = new Vector2D(1, 0);

            obj.Update(1 / 60.0);
            Assert.AreEqual(new Vector2D(1, 0), obj.Vertexs[0].Position);
            Assert.AreEqual(new Vector2D(2, 1), obj.Vertexs[2].Position);
            Assert.AreEqual(new Vector2D(0, 0), obj.Position);
        }

        [TestMethod]
        public void TestUpdateWithNotCloseObject()
        {
            // 封闭物体
            CombinedParticle obj = new CombinedParticle(
                new List<Vector2D>
                {
                    new Vector2D(0, 0),
                    new Vector2D(0, 1),
                    new Vector2D(1, 1)
                }, isClose: false);
            obj.Position = new Vector2D(1, 0);

            obj.Update(1 / 60.0);
            Assert.AreEqual(new Vector2D(1, 0), obj.Vertexs[0].Position);
            Assert.AreEqual(new Vector2D(2, 1), obj.Vertexs[2].Position);
            Assert.AreEqual(new Vector2D(0, 0), obj.Position);
        }
    }
}
