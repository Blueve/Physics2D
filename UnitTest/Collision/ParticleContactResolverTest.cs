using System;
using Physics2D.Collision;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Physics2D.Object;
using Physics2D.Common;

namespace UnitTest.Collision
{
    [TestClass]
    public class ParticleContactResolverTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var resolver = new ParticleContactResolver(100);

            Assert.AreEqual(100, resolver.Iterations);
        }

        [TestMethod]
        public void TestGetterAndSetterOfIterations()
        {
            var resolver = new ParticleContactResolver(100);
            resolver.Iterations = 1000;

            Assert.AreEqual(1000, resolver.Iterations);
        }

        [TestMethod]
        public void TestResolveContacts()
        {
            var resolver = new ParticleContactResolver(100);

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
            var contact = new ParticleContact
            {
                PA = p[0],
                PB = p[1],
                ContactNormal = new Vector2D(-1, 0),
                Penetration = 1,
                Restitution = 1
            };
            var contactList = new List<ParticleContact>();
            resolver.ResolveContacts(contactList, 1 / 60.0);
            
            contactList.Add(contact);
            resolver.ResolveContacts(contactList, 1 / 60.0);
            Assert.AreEqual(new Vector2D(-1, 0), p[0].Position, "物体0依据速度分量分离");
            Assert.AreEqual(new Vector2D(0, 0), p[0].Velocity, "物体0碰撞后速度相反");
            Assert.AreEqual(new Vector2D(1, 0), p[1].Velocity, "物体1碰撞后速度相反");

            p = new List<Particle>
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
            contactList = new List<ParticleContact>
            {
                new ParticleContact
                {
                    PA = p[1],
                    PB = p[0],
                    ContactNormal = new Vector2D(1, 0),
                    Penetration = 1,
                    Restitution = 1
                }
            };
            resolver.ResolveContacts(contactList, 1 / 60.0);
            Assert.AreEqual(new Vector2D(-1, 0), p[0].Position, "函数满足对称性");
            Assert.AreEqual(new Vector2D(0, 0), p[0].Velocity, "函数满足对称性");
            Assert.AreEqual(new Vector2D(1, 0), p[1].Velocity, "函数满足对称性");

            
        }

        public void TestResovleMultiContacts()
        {
            var resolver = new ParticleContactResolver(100);
            var p = new List<Particle>
            {
                new Particle
                {
                    Position = new Vector2D(0, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(2, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                },
                new Particle
                {
                    Position = new Vector2D(4, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contactList = new List<ParticleContact>
            {
                new ParticleContact
                {
                    PA = p[0],
                    PB = p[1],
                    ContactNormal = new Vector2D(-1, 0),
                    Penetration = 1,
                    Restitution = 1
                },
                new ParticleContact
                {
                    PA = p[2],
                    PB = p[1],
                    ContactNormal = new Vector2D(1, 0),
                    Penetration = 1,
                    Restitution = 1
                }
            };
            resolver.ResolveContacts(contactList, 1 / 60.0);
            Assert.AreEqual(new Vector2D(-1, 0), p[0].Position, "物体0向左分离");
            Assert.AreEqual(new Vector2D(2, 0), p[1].Position, "物体1位置不变");
            Assert.AreEqual(new Vector2D(5, 0), p[2].Position, "物体2向右分离");
        }

        [TestMethod]
        public void TestResolveContinuousContacts()
        {
            var resolver = new ParticleContactResolver(100);
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
                },
                new Particle
                {
                    Position = new Vector2D(4, 0),
                    Velocity = new Vector2D(0, 0),
                    Mass = 1
                }
            };
            var contactList = new List<ParticleContact>
            {
                new ParticleContact
                {
                    PA = p[0],
                    PB = p[1],
                    ContactNormal = new Vector2D(-1, 0),
                    Penetration = 1,
                    Restitution = 1
                },
                new ParticleContact
                {
                    PA = p[1],
                    PB = p[2],
                    ContactNormal = new Vector2D(-1, 0),
                    Penetration = 0,
                    Restitution = 1
                }
            };
            resolver.ResolveContacts(contactList, 1 / 60.0);
            Assert.AreEqual(new Vector2D(-1, 0), p[0].Position, "物体0向左分离");
            Assert.AreEqual(new Vector2D(2, 0), p[1].Position, "物体1位置不变");
            Assert.AreEqual(new Vector2D(4, 0), p[2].Position, "物体2位置不变");

            Assert.AreEqual(new Vector2D(0, 0), p[0].Velocity, "物体0碰撞后无速度");
            Assert.AreEqual(new Vector2D(0, 0), p[1].Velocity, "物体1碰撞后无速度");
            Assert.AreEqual(new Vector2D(1, 0), p[2].Velocity, "物体2获得物体0的速度");
        }
    }
}
