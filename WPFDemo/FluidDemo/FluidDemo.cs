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
using Physics2D.Collision.Basic;
using WPFDemo.Graphic;

namespace WPFDemo.FluidDemo
{
    public class FluidDemo : PhysicsGraphic
    {
        private readonly Water _water;

        private readonly Vector2D _center = new Vector2D(ConvertUnits.ToSimUnits(250f), ConvertUnits.ToSimUnits(200f));

        public FluidDemo(Image image)
            : base(image)
        {
            // 创建液体容器
            _water = new Water((int)image.Width, (int)image.Height);
            // 添加到绘制队列
            this.DrawQueue.Add(_water);
        }

        protected override void UpdatePhysics(double duration)
        {
            if(!_flag)
                foreach (var item in _water.ObjList)
                {
                    Vector2D v = _center - item.Position;
                    double d = v.Length();
                    item.AddForce(v.Normalize() * 5 * d);
                }
            PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            Random rnd = new Random();
            if (!Start)
            {
                Start = true;
                // 设置全局的阻力
                PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.5, 0.5));
                
                // 初始化水
                for (int i = 0; i < 20; i++)
                {
                    var item = PhysicsWorld.CreateParticle
                    (
                        new Vector2D
                        (
                            rnd.Next((int)Bitmap.Width).ToSimUnits(),
                            rnd.Next((int)Bitmap.Height).ToSimUnits()
                        ),
                        Vector2D.Zero,
                        1
                    );
                    _water.ObjList.Add(item);
                }
            }
            // 抖动
            _water.ObjList.ForEach(obj => obj.Velocity.Set(rnd.Next(5) - 2.5, rnd.Next(5) - 2.5));
        }

        private bool _flag = false;

        private ParticleBall _contactBall = new ParticleBall(0.02);
    }
}