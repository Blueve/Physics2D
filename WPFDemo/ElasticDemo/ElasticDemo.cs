namespace WPFDemo.ElasticDemo
{
    using System.Windows.Controls;
    using Physics2D;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Force;
    using WPFDemo.Graphic;

    public class ElasticDemo : PhysicsGraphic
    {
        /// <summary>
        /// 网格宽
        /// </summary>
        private const int Width = 20;

        /// <summary>
        /// 网格高
        /// </summary>
        private const int Height = 10;

        /// <summary>
        /// 延展系数
        /// </summary>
        private const double Factor = 4;

        /// <summary>
        /// 网格尺寸
        /// </summary>
        private static readonly double GridSize = 20.ToSimUnits();

        /// <summary>
        /// 弹性网
        /// </summary>
        private ElasticatedNet elasticatedNet;

        public ElasticDemo(Image image)
            : base(image)
        {
            // 设置全局的阻力用以增加稳定性
            this.PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.8f, 0.6f));
        }

        /// <summary>
        /// 更新物理世界
        /// </summary>
        /// <param name="duration"></param>
        protected override void UpdatePhysics(double duration)
        {
            this.PhysicsWorld.Update(duration);
        }

        /// <summary>
        /// 响应鼠标的操作
        /// </summary>
        public void Fire()
        {
            if (this.Start)
            {
                this.PhysicsWorld.RemoveObject(this.elasticatedNet);
                this.DrawQueue.Remove(this.elasticatedNet);
            }

            // 创建弹性网并加入到物理世界和绘制队列
            this.elasticatedNet = new ElasticatedNet(
                new Vector2D(40, 120).ToSimUnits(),
                Width, Height, GridSize, Factor);
            this.DrawQueue.Add(this.elasticatedNet);
            this.PhysicsWorld.AddObject(this.elasticatedNet);

            // 拉扯弹性网
            var net = this.elasticatedNet.Net;
            for (int i = 0; i < Width; i++)
            {
                net[i, Height - 1].Velocity = new Vector2D(0, 0.1) * 4 * (i >= Width / 2 ? -1 : 1);
                net[i, Height - 1].InverseMass = 0;
            }

            this.Start = true;
        }
    }
}