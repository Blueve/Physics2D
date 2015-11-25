using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Core;
using Physics2D.Factories;
using Physics2D.Common;
using Physics2D.Collision.Shapes;
using Physics2D.Common.Exceptions;
using System.Collections.Generic;
using Physics2D.Object;
using Physics2D.Collision.Basic;

namespace UnitTest.Factories
{
    [TestClass]
    public class ContactFactoryTest
    {
        [TestMethod]
        public void TestCreateEdge()
        {
            var world = new World();

            var edge = world.CreateEdge(new Vector2D(0, 0), new Vector2D(0, 5));
            TestEdgeProperty(edge, new Vector2D(0, 0), new Vector2D(0, 5));

            edge = world.CreateEdge(0, 0, 0, 5);
            TestEdgeProperty(edge, new Vector2D(0, 0), new Vector2D(0, 5));
        }



        private void TestEdgeProperty(Edge edge, Vector2D pointA, Vector2D pointB)
        {
            Assert.AreEqual(pointA, edge.PointA);
            Assert.AreEqual(pointB, edge.PointB);
            Assert.AreEqual(0, edge.Id);
            Assert.IsNull(edge.Body);
        }

        [TestMethod]
        public void TestCreatePolygonEdge()
        {
            var world = new World();

            try
            {
                IEnumerable<Edge> poly = null;
                try
                {
                    poly = world.CreatePolygonEdge(new Vector2D(0, 0), new Vector2D(0, 5), new Vector2D(5, 0));
                }
                catch(Exception)
                {
                    Assert.Fail("不应出现任何异常");
                }

                var it = poly.GetEnumerator();
                it.MoveNext();
                TestEdgeProperty(it.Current, new Vector2D(0, 0), new Vector2D(0, 5));

                it.MoveNext();
                TestEdgeProperty(it.Current, new Vector2D(0, 5), new Vector2D(5, 0));
                
                it.MoveNext();
                TestEdgeProperty(it.Current, new Vector2D(5, 0), new Vector2D(0, 0));
            }
            catch (Exception e)
            {
                Assert.Fail($"不应出现任何异常, 但实际上出现了{e.GetType().ToString()}");
            }

            try
            {
                var poly = world.CreatePolygonEdge(new Vector2D(0, 0), new Vector2D(0, 5));
            }
            catch (InvalidArgumentException) { }
            catch (Exception)
            {
                Assert.Fail($"不应出现除{nameof(InvalidArgumentException)}之外的异常");
            }
        }

        [TestMethod]
        public void TestCreateRod()
        {
            var world = new World();

            var rod = world.CreateRod(
                new Particle { Position = new Vector2D(0, 0) }, 
                new Particle { Position = new Vector2D(0, 5) });

            TestLinkProperty(rod, new Vector2D(0, 0), new Vector2D(0, 5));
            Assert.AreEqual(5, rod.Length);

        }

        private void TestLinkProperty(ParticleLink link, Vector2D pointA, Vector2D pointB)
        {
            Assert.AreEqual(pointA, link.ParticleA.Position);
            Assert.AreEqual(pointB, link.ParticleB.Position);
        }

        [TestMethod]
        public void TestCreateRope()
        {
            var world = new World();

            var rope = world.CreateRope(
                10,
                1,
                new Particle { Position = new Vector2D(0, 0) },
                new Particle { Position = new Vector2D(0, 5) });

            TestLinkProperty(rope, new Vector2D(0, 0), new Vector2D(0, 5));
            Assert.AreEqual(10, rope.MaxLength);
            Assert.AreEqual(1, rope.Restitution);
        }
    }
}
