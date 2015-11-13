using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Object;
using System.Collections.Generic;
using Physics2D.Common;
using Physics2D.Collision;

namespace UnitTest.Collision
{
    [TestClass]
    public class ParticleContactTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(1, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contact = new ParticleContact(p[0], p[1], 0.5, 1, new Vector2D(-1, 0));

            Assert.AreEqual(p[0], contact.PA, "PA");
            Assert.AreEqual(p[1], contact.PB, "PB");
            Assert.AreEqual(0.5, contact.Restitution, "Restitution");
            Assert.AreEqual(1, contact.Penetration, "Penetration");
            Assert.AreEqual(new Vector2D(-1, 0), contact.ContactNormal, "Contact Normal");
        }

        [TestMethod]
        public void TestCalculateSeparatingVelocityWithOneObject()
        {
            var p = new Particle
            {
                Position = new Vector2D(0, 0),
                Velocity = new Vector2D(1, 0),
                Mass = 1
            };
            var contact = new ParticleContact(p, null, 0.5, 1, new Vector2D(-1, 0));

            Assert.AreEqual(-1, contact.CalculateSeparatingVelocity());
        }

        [TestMethod]
        public void TestCalculateSeparatingVelocityWithTwoObject()
        {
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(1, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contact = new ParticleContact(p[0], p[1], 0.5, 1, new Vector2D(-1, 0));

            Assert.AreEqual(-1, contact.CalculateSeparatingVelocity());
        }

        [TestMethod]
        public void TestResolveVelocityWithOneObject()
        {
            var p = new Particle
            {
                Position = new Vector2D(0, 0),
                Velocity = new Vector2D(1, 0),
                Mass = 1
            };
            var contact = new ParticleContact(p, null, 1, 1, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveVelocity", 1 / 60.0);

            Assert.AreEqual(new Vector2D(-1, 0), p.Velocity);
        }

        [TestMethod]
        public void TestResolveVelocityWithTwoObject()
        {
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(1, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contact = new ParticleContact(p[0], p[1], 1, 1, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveVelocity", 1 / 60.0);

            Assert.AreEqual(new Vector2D(0, 0), p[0].Velocity, "物体0失去速度");
            Assert.AreEqual(new Vector2D(1, 0), p[1].Velocity, "物体1获得物体0碰撞前的速度");
        }

        [TestMethod]
        public void TestResolveVelocityOnSeparating()
        {
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(-1, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contact = new ParticleContact(p[0], p[1], 1, 1, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveVelocity", 1 / 60.0);

            Assert.AreEqual(new Vector2D(-1, 0), p[0].Velocity, "物体0速度不变");
            Assert.AreEqual(new Vector2D(0, 0), p[1].Velocity, "物体1速度不变");
        }

        [TestMethod]
        public void TestResolveVelocityWithTwoFixedObject()
        {
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(1, 0),
                    InverseMass = 0
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    InverseMass = 0
                }
            };
            var contact = new ParticleContact(p[0], p[1], 1, 1, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveVelocity", 1 / 60.0);

            Assert.AreEqual(new Vector2D(1, 0), p[0].Velocity, "物体0速度不变");
            Assert.AreEqual(new Vector2D(0, 0), p[1].Velocity, "物体1速度不变");
        }

        [TestMethod]
        public void TestResolveVelocityWithAcceleration()
        {
            var p = new Particle
            {
                Position = new Vector2D(0, 0),
                Velocity = new Vector2D(1, 0),
                Mass = 1
            };

            p.AddForce(new Vector2D(1, 0));
            p.Update(1);

            var contact = new ParticleContact(p, null, 1, 0, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveVelocity", 1);
            
            Assert.AreEqual(new Vector2D(-1, 0), p.Velocity, "物体获得补偿速度");

            p.AddForce(new Vector2D(1, 0));
            p.Update(1);
            Assert.AreEqual(new Vector2D(0, 0), p.Velocity, "物体保持静止");

            p.AddForce(new Vector2D(-1, 0));
            resolveVelocity.Invoke("ResolveVelocity", 1);
            Assert.AreEqual(new Vector2D(0, 0), p.Velocity, "物体不会获得补偿速度");
        }

        [TestMethod]
        public void TestResolveInterpenetrationWithOneObject()
        {
            var p = new Particle
            {
                Position = new Vector2D(0, 0),
                Velocity = new Vector2D(1, 0),
                Mass = 1
            };
            var contact = new ParticleContact(p, null, 1, 1, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveInterpenetration", 1 / 60.0);

            Assert.AreEqual(new Vector2D(-1, 0), p.Position);
        }

        [TestMethod]
        public void TestResolveInterpenetrationWithTwoObject()
        {
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(1, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contact = new ParticleContact(p[0], p[1], 1, 1, new Vector2D(-1, 0));
            var resolveVelocity = new PrivateObject(contact);
            resolveVelocity.Invoke("ResolveInterpenetration", 1 / 60.0);

            Assert.AreEqual(new Vector2D(-1, 0), p[0].Position, "物体0后退");
            Assert.AreEqual(new Vector2D(2, 0), p[1].Position, "物体1位置不变");
        }
    }
}
