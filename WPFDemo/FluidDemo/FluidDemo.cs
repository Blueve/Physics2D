namespace WPFDemo.FluidDemo
{
    using System;
    using System.Windows.Controls;
    using Physics2D;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Force;
    using WPFDemo.Graphic;

    public class FluidDemo : PhysicsGraphic
    {
        private readonly Water water;

        private readonly Vector2D center = new Vector2D(ConvertUnits.ToSimUnits(250f), ConvertUnits.ToSimUnits(200f));

        public FluidDemo(Image image)
            : base(image)
        {
            // 创建液体容器
            this.water = new Water((int)image.Width, (int)image.Height);

            // 添加到绘制队列
            this.DrawQueue.Add(this.water);
        }

        protected override void UpdatePhysics(double duration)
        {
            if (!this.flag)
                foreach (var item in this.water.ObjList)
                {
                    Vector2D v = this.center - item.Position;
                    double d = v.Length();
                    item.AddForce(v.Normalize() * 5 * d);
                }

            this.PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            Random rnd = new Random();
            if (!this.Start)
            {
                this.Start = true;

                // 设置全局的阻力
                this.PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.5, 0.5));

                // 初始化水
                for (int i = 0; i < 20; i++)
                {
                    var item = this.PhysicsWorld.CreateParticle(
                        new Vector2D(
                            rnd.Next((int)this.Bitmap.Width).ToSimUnits(),
                            rnd.Next((int)this.Bitmap.Height).ToSimUnits()),
                        Vector2D.Zero,
                        1);
                    item.BindShape(new Circle(2.ToSimUnits()));
                    this.water.ObjList.Add(item);
                }
            }

            // 抖动
            this.water.ObjList.ForEach(obj => obj.Velocity.Set(rnd.Next(5) - 2.5, rnd.Next(5) - 2.5));
        }

        private bool flag = false;

    }
}