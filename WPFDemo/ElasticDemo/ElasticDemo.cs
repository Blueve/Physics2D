using Physics2D.Common;
using System.Windows.Controls;
using WPFDemo.Graphic;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D;

namespace WPFDemo.ElasticDemo
{
    public class ElasticDemo : PhysicsGraphic
    {
        #region 静态值及常量
        /// <summary>
        /// 网格宽
        /// </summary>
        private const int Width = 20;
        /// <summary>
        /// 网格高
        /// </summary>
        private const int Height = 10;
        /// <summary>
        /// 网格尺寸
        /// </summary>
        private static readonly double GridSize = 20.ToSimUnits();
        #endregion

        #region 私有字段
        /// <summary>
        /// 弹性网
        /// </summary>
        private ElasticatedNet _elasticatedNet;
        #endregion

        #region 构造方法
        public ElasticDemo(Image image)
            : base(image)
        {
            // 设置全局的阻力用以增加稳定性
            PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.8f, 0.6f));
        }
        #endregion

        #region 实现PhysicsGraphic
        /// <summary>
        /// 更新物理世界
        /// </summary>
        /// <param name="duration"></param>
        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }
        #endregion

        #region 公开的方法
        /// <summary>
        /// 响应鼠标的操作
        /// </summary>
        public void Fire()
        {
            if(Start)
            {
                PhysicsWorld.RemoveObject(_elasticatedNet);
                DrawQueue.Remove(_elasticatedNet);
            }

            // 创建弹性网并加入到物理世界和绘制队列
            _elasticatedNet = new ElasticatedNet(
                new Vector2D(40, 120).ToSimUnits(),
                Width, Height, GridSize
            );
            DrawQueue.Add(_elasticatedNet);
            PhysicsWorld.AddObject(_elasticatedNet);

            // 拉扯弹性网
            var net = _elasticatedNet.Net;
            for (int i = 0; i < Width; i++)
            {
                net[i, Height - 1].Velocity = new Vector2D(0, 0.1) * 4 * (i >= Width / 2 ? -1 : 1);
                net[i, Height - 1].InverseMass = 0;
            }

            Start = true;
        }
        #endregion
    }
}