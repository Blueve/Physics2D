namespace WPFDemo.Graphic
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Physics2D.Core;

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
            this.Bitmap = BitmapFactory.New((int)image.Width, (int)image.Height);
        }

        public void Update(object sender, EventArgs e)
        {
            var interval = this.TimeTracker.Update();
            if (!this.Start)
            {
                return;
            }

            // 更新物理世界
            this.TimeSpan += interval;
            for (; this.TimeSpan >= this.Slot; this.TimeSpan -= this.Slot)
            {
                this.UpdatePhysics(this.Slot);
            }

            // 更新图形
            this.Bitmap.Clear();
            this.DrawQueue.ForEach(item => item.Draw(this.Bitmap));
        }

        /// <summary>
        /// 更新物理世界
        /// </summary>
        /// <param name="duration">持续时间</param>
        protected abstract void UpdatePhysics(double duration);
    }
}