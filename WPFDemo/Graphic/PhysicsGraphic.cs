using Physics2D.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFDemo.Graphic
{
    public abstract class PhysicsGraphic
    {
        /// <summary>
        /// 物理世界
        /// </summary>
        protected World physicsWorld = new World();

        /// <summary>
        /// 帧率控制计时器
        /// </summary>
        protected TimeTracker timeTracker = new TimeTracker();

        protected double timeSpan = 0;

        /// <summary>
        /// 是否渲染标记
        /// </summary>
        public bool Start = false;

        /// <summary>
        /// 绘制层
        /// </summary>
        public readonly WriteableBitmap bitmap;

        /// <summary>
        /// 绘制队列
        /// </summary>
        protected readonly List<IDrawable> drawQueue = new List<IDrawable>();

        /// <summary>
        /// 物理演算时间槽大小
        /// </summary>
        protected double slot = 1 / 60.0;

        public PhysicsGraphic(Image image)
        {
            bitmap = BitmapFactory.New((int)image.Width, (int)image.Height);
        }

        public void Update(object sender, EventArgs e)
        {
            float interval = (float)timeTracker.Update();
            if (!Start) return;

            // 更新物理世界
            timeSpan += interval;
            for (; timeSpan >= slot; timeSpan -= slot)
            {
                UpdatePhysics(slot);
            }
            // 更新图形
            bitmap.Clear();
            foreach (var item in drawQueue)
            {
                item.Draw(bitmap);
            }
        }

        /// <summary>
        /// 更新物理世界
        /// </summary>
        /// <param name="duration">持续时间</param>
        protected abstract void UpdatePhysics(double duration);
    }
}