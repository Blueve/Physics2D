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
        /// 绘制层
        /// </summary>
        public readonly WriteableBitmap Bitmap;

        /// <summary>
        /// 绘制队列
        /// </summary>
        protected readonly List<IDrawable> DrawQueue = new List<IDrawable>();

        /// <summary>
        /// 物理演算时间槽大小
        /// </summary>
        protected double Slot = 1 / 60.0;

        protected PhysicsGraphic(Image image)
        {
            Bitmap = BitmapFactory.New((int)image.Width, (int)image.Height);
        }

        public void Update(object sender, EventArgs e)
        {
            float interval = (float)TimeTracker.Update();
            if (!Start) return;

            // 更新物理世界
            TimeSpan += interval;
            for (; TimeSpan >= Slot; TimeSpan -= Slot)
            {
                UpdatePhysics(Slot);
            }
            // 更新图形
            Bitmap.Clear();
            DrawQueue.ForEach(item => item.Draw(Bitmap));
        }

        /// <summary>
        /// 更新物理世界
        /// </summary>
        /// <param name="duration">持续时间</param>
        protected abstract void UpdatePhysics(double duration);
    }
}