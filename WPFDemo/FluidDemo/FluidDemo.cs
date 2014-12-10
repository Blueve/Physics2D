using Physics2D;
using Physics2D.Collision;
using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFDemo.Graphic;

namespace WPFDemo.FluidDemo
{
    public class FluidDemo : PhysicsGraphic
    {
        private Water water;

        private Vector2D center = new Vector2D(ConvertUnits.ToSimUnits(250f), ConvertUnits.ToSimUnits(200f));

        public FluidDemo(Image image)
            : base(image)
        {
            // 创建液体容器
            water = new Water((int)image.Width, (int)image.Height);
            // 添加到绘制队列
            this.drawQueue.Add(water);
        }

        protected override void UpdatePhysics(float duration)
        {
            if(!flag)
                foreach (var item in water.objList)
                {
                    Vector2D v = center - item.Position;
                    float d = v.Length();
                    item.AddForce(v.Normalize() * 5f * d);
                }
            physicsWorld.Update(duration);
        }

        public void Fire()
        {
            Random rnd = new Random();
            if (!Start)
            {
                Start = true;
                // 设置全局的阻力
                ZoneFactory.CreateGlobalZone(physicsWorld, new ParticleDrag(0.5f, 0.5f));
                physicsWorld.RegistryContactGenerator(contactBall);
                physicsWorld.RegistryContactGenerator(contact);
                physicsWorld.RegistryContactGenerator(contact2);
                
                // 初始化水
                for (int i = 0; i < 100; i++)
                {
                    var item = physicsWorld.CreateParticle
                    (
                        new Vector2D
                        (
                            ConvertUnits.ToSimUnits(rnd.Next((int)bitmap.Width)),
                            ConvertUnits.ToSimUnits(rnd.Next((int)bitmap.Height))
                        ),
                        Vector2D.Zero,
                        1f
                    );
                    water.objList.Add(item);
                    contactBall.AddBall(item, ConvertUnits.ToSimUnits(3));
                    contact.AddBall(item, ConvertUnits.ToSimUnits(30));
                    contact2.AddBall(item, ConvertUnits.ToSimUnits(30));
                }
            }
            else
            {
                ZoneFactory.CreateGlobalZone(physicsWorld, new ParticleGravity(new Vector2D(0f, 10f)));
                flag = true;
            }
            // 抖动
            foreach (var obj in water.objList)
            {
                obj.Velocity = new Vector2D(rnd.Next(5) - 2.5f, rnd.Next(5) - 2.5f);
            }
        }

        private bool flag = false;

        private ParticleBall contactBall = new ParticleBall(0.02f);

        private ParticleEdge contact = new ParticleEdge(0f,
                                                        ConvertUnits.ToSimUnits(0f),
                                                        ConvertUnits.ToSimUnits(350f),
                                                        ConvertUnits.ToSimUnits(500f),
                                                        ConvertUnits.ToSimUnits(400f));
        private ParticleEdge contact2 = new ParticleEdge(0f,
                                                        ConvertUnits.ToSimUnits(0f),
                                                        ConvertUnits.ToSimUnits(400f),
                                                        ConvertUnits.ToSimUnits(500f),
                                                        ConvertUnits.ToSimUnits(350f));
    }
}