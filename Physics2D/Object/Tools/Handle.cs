using Physics2D.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Object.Tools
{
    public class Handle : INotifyPropertyChanged
    {
        #region 私有字段
        /// <summary>
        /// 位置
        /// </summary>
        private Vector2D _position;
        #endregion

        #region 公开属性
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2D Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }
        #endregion

        #region 构造方法
        public Handle(Vector2D position)
        {
            _position = position;
        }
        #endregion

        #region 属性变动通知
        /// <summary>
        /// 设置属性值
        /// 当值发生变动时会自动触发属性变动事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storge"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storge, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storge, value)) return false;
            storge = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 触发属性变动事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region 公开方法
        /// <summary>
        /// 释放所有委托
        /// </summary>
        public void Release()
        {
            var delegates = PropertyChanged.GetInvocationList();
            foreach(var d in delegates)
            {
                PropertyChanged -= d as PropertyChangedEventHandler;
            }
        }
        #endregion

        #region 属性变动事件
        /// <summary>
        /// 属性变动事件
        /// 注册Handle发生变动时所需执行的委托
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
