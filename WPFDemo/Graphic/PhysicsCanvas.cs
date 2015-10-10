using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Core;
using SharpDX;
using SharpDX.Direct2D1;
using WPFDemo.Controls;

namespace WPFDemo.Graphic
{
    public abstract class PhysicsCanvas : Direct2DControl
    {
        /// <summary>
        /// 物理世界
        /// </summary>
        protected World PhysicsWorld = new World();

        /// <summary>
        /// 帧率控制计时器
        /// </summary>
        protected TimeTracker TimeTracker = new TimeTracker();

        protected double TimeSpan = 0;

        /// <summary>
        /// 是否渲染标记
        /// </summary>
        public bool Start = false;

        /// <summary>
        /// 物理演算时间槽大小
        /// </summary>
        protected double Slot = 1 / 60.0;
        
        /// <summary>
        /// 绘制队列
        /// </summary>
        protected readonly List<IRenderable> RenderQueue = new List<IRenderable>();

        public override void OnRender(RenderTarget target)
        {
            var interval = TimeTracker.Update();
            if (!Start) return;

            // 更新物理世界
            TimeSpan += interval;
            for (; TimeSpan >= Slot; TimeSpan -= Slot)
            {
                UpdatePhysics(Slot);
            }
            // 更新图形
            target.Clear(Color.White);
            RenderQueue.ForEach(item => item.Render(target));
        }

        /// <summary>
        /// 更新物理世界
        /// </summary>
        /// <param name="duration">持续时间</param>
        protected abstract void UpdatePhysics(double duration);
    }
}
