using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision.Shapes
{
    public abstract class Shape
    {
        /// <summary>
        /// 绑定的物体
        /// </summary>
        public PhysicsObject Body;

        /// <summary>
        /// 标识符
        /// 标识符相同且不为0的形状不执行碰撞检测
        /// </summary>
        public int Id = 0;

        /// <summary>
        /// 标识符基数
        /// </summary>
        private static int _idBase = 1;

        /// <summary>
        /// 产生一个新的Id
        /// </summary>
        /// <returns></returns>
        public static int NewId() => _idBase++;

        /// <summary>
        /// 返回当前形状的类型
        /// </summary>
        public abstract ShapeType Type
        {
            get;
        }
    }
}
