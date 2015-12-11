using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics2D.Core;
using Physics2D.Object;
using System.Collections.Generic;
using Physics2D.Common;
using Physics2D.Force.Zones;
using Physics2D.Force;
using Physics2D.Object.Tools;
using Physics2D.Collision.Shapes;

namespace UnitTest.Core
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        public void TestUpdate()
        {
            var world = new World();

            // 两种物体
            var objA = new CombinedParticle(
                new List<Vector2D> {
                    new Vector2D(0, 0),
                    new Vector2D(5, 0),
                    new Vector2D(0, 5)
                });
            var objB = new Particle { Mass = 1, Position = Vector2D.Zero };
            
            // 一个全局作用力
            var zone = new GlobalZone();
            zone.Add(new ParticleGravity(new Vector2D(0, 9.8)));
            world.Zones.Add(zone);

            TestAddCustomObject(world, objA);
            var handle = TestPin(world, objA);
            TestUnPin(world, objA, handle);
            TestRemoveCustomObject(world, objA);

            // 一个作用力
            var force = new ParticleConstantForce(new Vector2D(5, 0));
            force.Add(objB);
            world.ForceGenerators.Add(force);

            TestAddObject(world, objB);
            TestRemoveObject(world, objB);
        }

        [TestMethod]
        public void TestAddAndRemoveEdge()
        {
            var world = new World();
            var obj = new Particle { Mass = 1, Position = new Vector2D(0, 5), Velocity = new Vector2D(0, 5) };
            obj.BindShape(new Circle(2));
            var edgeA = new Edge(-5, 0, 5, 0);
            var edgeB = new Edge(-5, 10, 5, 10);

            world.AddObject(obj);
            world.AddEdge(edgeA);
            world.AddEdge(edgeB);

            world.Update(1);
            Assert.AreEqual(new Vector2D(0, -5), obj.Velocity, "质体会被反弹");

            world.Update(1);
            world.Update(1);
            Assert.AreEqual(new Vector2D(0, 5), obj.Velocity, "质体会被再次反弹");

            world.RemoveEdge(edgeB);
            world.Update(1);
            world.Update(1);
            Assert.AreEqual(new Vector2D(0, 5), obj.Velocity, "移除边缘后质体不会被反弹");
        }

        private void TestAddCustomObject(World world, CombinedParticle obj)
        {
            world.AddObject(obj);
            world.Update(1);

            var vertexs = obj.Vertexs;
            for(var i = 0; i < vertexs.Count; i++)
            {
                Assert.AreEqual(new Vector2D(0, 9.8), vertexs[i].Acceleration, $"作用力应被正确地施加到了物体{i}上");
            }
        }

        private Handle TestPin(World world, CombinedParticle obj)
        {
            obj.Position = Vector2D.Zero;
            var handle = world.Pin(obj, Vector2D.Zero);

            try
            {
                world.Pin(obj, Vector2D.UnitX);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(InvalidOperationException), "不允许对同一物体Pin多次");
            }

            handle.Position = new Vector2D(100, 100);
            Assert.AreEqual(new Vector2D(100, 100), obj.Position, "Handle能对其生效");

            return handle;
        }

        private void TestUnPin(World world, CombinedParticle obj, Handle handle)
        {
            obj.Position = Vector2D.Zero;
            world.UnPin(obj);
            handle.Position = new Vector2D(100, 100);
            Assert.AreEqual(new Vector2D(0, 0), obj.Position, "Handle不能对其生效");

            try
            {
                world.UnPin(obj);
            }
            catch(Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(InvalidOperationException), "不允许对未Pin的物体执行UnPin");
            }
        }

        private void TestRemoveCustomObject(World world, CombinedParticle obj)
        {
            var vertexs = obj.Vertexs;
            foreach(var vertex in vertexs)
            {
                vertex.Acceleration = Vector2D.Zero;
            }

            world.RemoveObject(obj);
            world.Update(1);
            
            for (var i = 0; i < vertexs.Count; i++)
            {
                Assert.AreEqual(new Vector2D(0, 0), vertexs[i].Acceleration, $"物体{i}上");
            }
        }

        private void TestAddObject(World world, PhysicsObject obj)
        {
            world.AddObject(obj);
            world.Update(1);

            Assert.AreEqual(new Vector2D(5, 9.8), obj.Acceleration, "作用力应被正确地施加到了物体上");
        }

        private void TestRemoveObject(World world, PhysicsObject obj)
        {
            obj.Acceleration = Vector2D.Zero;
            world.RemoveObject(obj);
            world.Update(1);

            Assert.AreEqual(new Vector2D(0, 0), obj.Acceleration, "作用力不再作用于物体上");
        }
    }
}
