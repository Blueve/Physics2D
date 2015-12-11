using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Core;
using Physics2D.Collision;
using Physics2D.Collision.Shapes;
using Physics2D.Object;
using Physics2D.Common;
using Physics2D;
using System.Collections.Generic;
using Physics2D.Collision.Basic;

namespace UnitTest.Core
{
    [TestClass]
    public class ContactRegistryTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var objects = new HashSet<PhysicsObject>
            {
                new Particle { Mass = 1, Position = new Vector2D(0, 0) },
                new Particle { Mass = 1, Position = new Vector2D(2, 0) }
            };
            
            var contactRegistry = new ContactRegistry(objects, new HashSet<Edge>());
            Assert.IsNotNull(contactRegistry);
        }

        [TestMethod]
        public void TestDispatchToDetector()
        {
            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };

            pA.BindShape(new Circle(5));
            pB.BindShape(new Circle(5));

            var contact = ContactRegistry.DispatchToDetector(
                ContactRegistry.ContactTypeMap[(int)pA.Shape.Type, (int)pB.Shape.Type],
                pA.Shape, pB.Shape);
            Assert.IsNotNull(contact, "发生圆圆碰撞");

            var edge = new Edge(0, 4, 4, 4);
            contact = ContactRegistry.DispatchToDetector(
                ContactRegistry.ContactTypeMap[(int)pA.Shape.Type, (int)edge.Type],
                pA.Shape, edge);
            Assert.IsNotNull(contact, "发生圆边碰撞");
        }

        [TestMethod]
        public void TestCollectAllShapes()
        {
            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };

            pA.BindShape(new Circle(5));
            pB.BindShape(new Point());
            var objects = new HashSet<PhysicsObject> {  pA, pB };
            var edges = new HashSet<Edge> { new Edge(0, 4, 4, 4) };

            var shapes = ContactRegistry.CollectAllShapes(objects, edges);
            Assert.AreEqual(2 ,shapes.Count);
        }

        [TestMethod]
        public void TestExcuteParticleCollisionDetector()
        {
            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };

            pA.BindShape(new Circle(5));
            pB.BindShape(new Circle(5));
            var objects = new HashSet<PhysicsObject> { pA, pB };
            var edge = new Edge(0, 4, 4, 4);
            var edges = new HashSet<Edge> { edge };

            var shapes = ContactRegistry.CollectAllShapes(objects, edges);
            var contacts = ContactRegistry.ExcuteParticleCollisionDetector(shapes);
            var count = 0;
            while (contacts.MoveNext()) count++;
            Assert.AreEqual(3, count, "应当产生三个碰撞");

            shapes = new List<Shape> { edge, pA.Shape, pB.Shape };
            contacts = ContactRegistry.ExcuteParticleCollisionDetector(shapes);
            count = 0;
            while (contacts.MoveNext()) count++;
            Assert.AreEqual(3, count, "形状列表的次序不影响结果");
            
            pA.BindShape(new Circle(5, 1));
            pB.BindShape(new Circle(5, 1));
            shapes = new List<Shape> { pA.Shape, edge, pB.Shape };
            contacts = ContactRegistry.ExcuteParticleCollisionDetector(shapes);
            count = 0;
            while (contacts.MoveNext()) count++;
            Assert.AreEqual(2, count, "形状标识符一致的物体不执行碰撞检测");
        }

        [TestMethod]
        public void TestResolveContacts()
        {
            Settings.ContactIteration = 1;

            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };
            pA.BindShape(new Circle(5));
            pB.BindShape(new Circle(5));
            var objects = new HashSet<PhysicsObject> { pA, pB };

            var edge = new Edge(0, 4, 4, 4);
            var edges = new HashSet<Edge> { edge };

            var contactRegistry = new ContactRegistry(objects, edges);
            contactRegistry.OnContactEvent += (s, a) =>
            {
                Assert.AreEqual(contactRegistry, s);
                Assert.AreEqual(3, a.ContactList.Count);
            };

            contactRegistry.ResolveContacts(1 / 60.0);
        }

        [TestMethod]
        public void TestResolveContactsWithContactGenerator()
        {
            Settings.ContactIteration = 1;

            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };
            pA.BindShape(new Circle(5));
            pB.BindShape(new Circle(5));
            
            var contactRegistry = new ContactRegistry(new HashSet<PhysicsObject>(), new HashSet<Edge>());
            contactRegistry.Add(new ParticleRope(2, 0, pA, pB));
            contactRegistry.OnContactEvent += (s, a) =>
            {
                Assert.AreEqual(contactRegistry, s);
                Assert.AreEqual(1, a.ContactList.Count);
            };
            contactRegistry.ResolveContacts(1 / 60.0);
        }

        [TestMethod]
        public void TestResolveContactsNotAddMoreContact()
        {
            Settings.ContactIteration = 1;
            Settings.MaxContacts = 0;

            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };
            pA.BindShape(new Circle(5));
            pB.BindShape(new Circle(5));

            var contactRegistry = new ContactRegistry(new HashSet<PhysicsObject>(), new HashSet<Edge>());
            contactRegistry.Add(new ParticleRope(2, 0, pA, pB));
            contactRegistry.OnContactEvent += (s, a) =>
            {
                Assert.AreEqual(contactRegistry, s);
                Assert.AreEqual(0, a.ContactList.Count);
            };
            contactRegistry.ResolveContacts(1 / 60.0);

            Settings.MaxContacts = 500;
        }

        [TestMethod]
        public void TestResolveContactsStopIteration()
        {
            Settings.ContactIteration = 2;

            var pA = new Particle { Mass = 1, Position = new Vector2D(0, 0) };
            var pB = new Particle { Mass = 1, Position = new Vector2D(2, 0) };
            pA.BindShape(new Circle(5));
            pB.BindShape(new Circle(5));
            var objects = new HashSet<PhysicsObject> { pA, pB };
           

            var contactRegistry = new ContactRegistry(objects, new HashSet<Edge>());
            contactRegistry.OnContactEvent += (s, a) =>
            {
                _isRan = true;
                Assert.AreEqual(contactRegistry, s);
                Assert.AreEqual(1, a.ContactList.Count);
            };

            contactRegistry.ResolveContacts(1 / 60.0);
            Assert.IsTrue(_isRan);
        }

        private bool _isRan = false;
    }
}
