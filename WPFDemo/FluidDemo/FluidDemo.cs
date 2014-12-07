using Physics2D;
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
                ZoneFactory.CreateGloablZone(physicsWorld, new ParticleDrag(0.5f, 0.5f));
                // 初始化水
                for (int i = 0; i < 20; i++)
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
                }
            }
            // 抖动
            foreach (var obj in water.objList)
            {
                obj.Velocity = new Vector2D(rnd.Next(5) - 2.5f, rnd.Next(5) - 2.5f);
            }
        }
    }
}