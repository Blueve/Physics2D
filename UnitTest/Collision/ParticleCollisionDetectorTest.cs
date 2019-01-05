namespace UnitTest.Collision
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Physics2D.Collision;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Object;

    [TestClass]
    public class ParticleCollisionDetectorTest
    {
        [TestMethod]
        public void TestCircleAndCircleCollided()
        {
            Particle pA = new Particle { Position = new Vector2D(0, 0), Restitution = 1 };
            Particle pB = new Particle { Position = new Vector2D(0, 3), Restitution = 0.5 };

            pA.BindShape(new Circle(2));
            pB.BindShape(new Circle(2));

            TestDetectorsResult(
                new ParticleContact(pA, pB, 0.75, 1, new Vector2D(0, -1)),
                ParticleCollisionDetector.CircleAndCircle(pA.Shape as Circle, pB.Shape as Circle));
        }

        [TestMethod]
        public void TestCircleAndCircleNotCollided()
        {
            Particle pA = new Particle { Position = new Vector2D(0, 0) };
            Particle pB = new Particle { Position = new Vector2D(0, 3) };

            pA.BindShape(new Circle(1));
            pB.BindShape(new Circle(1));

            Assert.IsNull(ParticleCollisionDetector.CircleAndCircle(pA.Shape as Circle, pB.Shape as Circle));
        }

        [TestMethod]
        public void TestCircleAndEdgeCollided()
        {
            Particle pA = new Particle { Position = new Vector2D(0, 2), Restitution = 1 };
            Edge edge = new Edge(0, 0, 5, 0);
            pA.BindShape(new Circle(5));

            TestDetectorsResult(
                new ParticleContact(pA, null, 1, 3, new Vector2D(0, 1)),
                ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心投影在边沿上");

            pA.Position = new Vector2D(-1, 2);
            TestDetectorsResult(
                new ParticleContact(pA, null, 1, 5 - Math.Sqrt(5), new Vector2D(-1, 2).Normalize()),
                ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心投影在边沿左延长线上");

            pA.Position = new Vector2D(6, 2);
            TestDetectorsResult(
                new ParticleContact(pA, null, 1, 5 - Math.Sqrt(5), new Vector2D(1, 2).Normalize()),
                ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心投影在边沿右延长线上");

            pA.Position = new Vector2D(-1, 0);
            TestDetectorsResult(
                new ParticleContact(pA, null, 1, 4, new Vector2D(-1, 0)),
                ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心在边沿的延长线上");

            pA.PrePosition = new Vector2D(2.5, 10);
            pA.Position = new Vector2D(2.5, -10);
            TestDetectorsResult(
                new ParticleContact(pA, null, 1, 5, new Vector2D(0, 1)),
                ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "发生了穿越");
        }

        [TestMethod]
        public void TestCircleAndEdgeNotCollided()
        {
            Particle pA = new Particle { Position = new Vector2D(-1, 6), Restitution = 1 };
            Edge edge = new Edge(0, 0, 5, 0);
            pA.BindShape(new Circle(5));

            Assert.IsNull(ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心投影在边沿左延长线上");

            pA.Position = new Vector2D(6, 6);
            Assert.IsNull(ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心投影在边沿右延长线上");

            pA.Position = new Vector2D(2.5, 6);
            Assert.IsNull(ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心投影在边沿上");

            pA.Position = new Vector2D(-6, 0);
            Assert.IsNull(ParticleCollisionDetector.CircleAndEdge(pA.Shape as Circle, edge), "圆心在边沿的延长线上");
        }

        private static void TestDetectorsResult(
            ParticleContact expectContact,
            ParticleContact contact,
            string message = "")
        {
            Assert.IsNotNull(contact, $"{message} 产生了碰撞");
            Assert.AreEqual(expectContact.PA, contact.PA, $"{message} 物体A");
            Assert.AreEqual(expectContact.PB, contact.PB, $"{message} 物体B");
            Assert.AreEqual(expectContact.ContactNormal, contact.ContactNormal, $"{message} 碰撞法线");
            Assert.AreEqual(expectContact.Restitution, contact.Restitution, $"{message} 回弹系数");
            Assert.AreEqual(expectContact.Penetration, contact.Penetration, $"{message} 相交深度");
        }
    }
}
