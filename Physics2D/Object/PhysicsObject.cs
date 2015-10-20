using Physics2D.Common;
using Physics2D.Collision.Shapes;
using System.Collections.Generic;
using System;

namespace Physics2D.Object
{
    public abstract class PhysicsObject : IUpdatable
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2D Position;

        /// <summary>
        /// 速度
        /// </summary>
        public Vector2D Velocity;

        /// <summary>
        /// 加速度
        /// </summary>
        public Vector2D Acceleration;

        /// <summary>
        /// 上一帧的位置
        /// </summary>
        public Vector2D PrePosition;

        /// <summary>
        /// 质量
        /// </summary>
        public double Mass
        {
            set
            {
                if (value != 0)
                {
                    _mass = value;
                    _inverseMass = 1.0 / value;
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            get { return _mass; }
        }

        /// <summary>
        /// 质量的倒数
        /// </summary>
        public double InverseMass
        {
            set
            {
                _mass = value == 0 ? double.MaxValue : 1.0 / value;
                _inverseMass = value;
            }
            get { return _inverseMass; }
        }

        protected double _mass;
        protected double _inverseMass;

        /// <summary>
        /// 物体绑定的形状
        /// </summary>
        private Shape _shape;

        /// <summary>
        /// 为物体绑定一个形状
        /// </summary>
        /// <param name="shape"></param>
        public void BindShape(Shape shape)
        {
            shape.body = this;
            _shape = shape;
        }

        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="duration"></param>
        public abstract void Update(double duration);
    }
}