using Physics2D.Common;
using System.Windows.Controls;
using WPFDemo.Graphic;

namespace WPFDemo.ElasticDemo
{
    public class ElasticDemo : PhysicsGraphic
    {
        private ElasticatedNet elasticatedNet;

        public ElasticDemo(Image image)
            : base(image)
        {
            // 创建弹性网
            elasticatedNet = new ElasticatedNet(
                physicsWorld,
                new Vector2D(40f / 50, 200f / 50),
                20,
                10,
                20f / 50
            );
            // 将弹性网加入绘制队列
            drawQueue.Add(elasticatedNet);
        }

        protected override void UpdatePhysics(float duration)
        {
            physicsWorld.Update(duration);
        }

        public void Fire()
        {
            Start = true;
            elasticatedNet.Reset();
        }
    }
}