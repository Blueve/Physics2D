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

        protected override void UpdatePhysics(double duration)
        {
            if(!flag)
                foreach (var item in water.objList)
                {
                    Vector2D v = center - item.Position;
                    double d = v.Length();
                    item.AddForce(v.Normalize() * 5 * d);
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
                physicsWorld.CreateGlobalZone(new ParticleDrag(0.5, 0.5));
                
                // 初始化水
                for (int i = 0; i < 100; i++)
                {
                    var item = physicsWorld.CreateParticle
                    (
                        new Vector2D
                        (
                            rnd.Next((int)bitmap.Width).ToSimUnits(),
                            rnd.Next((int)bitmap.Height).ToSimUnits()
                        ),
                        Vector2D.Zero,
                        1f
                    );
                    water.objList.Add(item);
                    //contactBall.AddBall(item, ConvertUnits.ToSimUnits(3));
                }
            }
            // 抖动
            foreach (var obj in water.objList)
            {
                obj.Velocity.Set(rnd.Next(5) - 2.5, rnd.Next(5) - 2.5);
            }
        }

        private bool flag = false;

        private ParticleBall contactBall = new ParticleBall(0.02);
    }
}