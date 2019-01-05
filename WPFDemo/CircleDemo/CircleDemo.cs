namespace WPFDemo.CircleDemo
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Physics2D;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Force;
    using Physics2D.Object;
    using WPFDemo.Graphic;

    public class CircleDemo : PhysicsGraphic, IDrawable
    {
        /// <summary>
        /// 中心点
        /// </summary>
        private readonly Particle centerObj;

        /// <summary>
        /// 物体列表
        /// </summary>
        private readonly List<Particle> objList = new List<Particle>();

        public CircleDemo(Image image)
            : base(image)
        {
            // 初始化中心点
            this.centerObj = this.PhysicsWorld.CreateFixedParticle(
                new Vector2D(
                    250.ToSimUnits(),
                    200.ToSimUnits()));

            // 注册绘制对象
            this.DrawQueue.Add(this);
        }

        protected override void UpdatePhysics(double duration)
        {
            foreach (var item in this.objList)
            {
                var v = this.centerObj.Position - item.Position;
                item.AddForce(v.Normalize() * 30);
            }

            this.PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            if (!this.Start)
            {
                // 全局增加一个小阻尼
                this.PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.01, 0.02));
                this.Start = true;
            }

            var item = this.PhysicsWorld.CreateParticle(
                new Vector2D(200.ToSimUnits(), 200.ToSimUnits()),
                new Vector2D(0, 5),
                1);
            this.objList.Add(item);
        }

        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.FillEllipseCentered(
                this.centerObj.Position.X.ToDisplayUnits(),
                this.centerObj.Position.Y.ToDisplayUnits(), 6, 6, Colors.Red);
            for (int i = this.objList.Count - 1; i >= 0; i--)
            {
                int x = this.objList[i].Position.X.ToDisplayUnits();
                int y = this.objList[i].Position.Y.ToDisplayUnits();

                bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
            }
        }
    }
}