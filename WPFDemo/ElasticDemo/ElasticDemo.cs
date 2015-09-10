using Physics2D.Common;
using System.Windows.Controls;
using WPFDemo.Graphic;
using Physics2D.Force;
using Physics2D.Factories;

namespace WPFDemo.ElasticDemo
{
    public class ElasticDemo : PhysicsGraphic
    {
        private readonly ElasticatedNet _elasticatedNet;

        public ElasticDemo(Image image)
            : base(image)
        {
            // 创建弹性网
            _elasticatedNet = new ElasticatedNet(
                PhysicsWorld,
                new Vector2D(40.0 / 50, 200.0 / 50),
                20,
                10,
                20.0 / 50
            );
            // 将弹性网加入绘制队列
            DrawQueue.Add(_elasticatedNet);

            // 设置全局的阻力
            PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.8f, 0.6f));
        }

        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            Start = true;
            _elasticatedNet.Reset();
        }
    }
}