using Physics2D.Common;
using Physics2D.Collision.Shapes;
using System.Collections.Generic;
using System;

namespace Physics2D.Object
{
    public abstract class PhysicsObject : IUpdatable
    {
        #region 公开的字段
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
        /// 碰撞回弹系数
        /// </summary>
        public double Restitution = 1;
        #endregion

        #region 保护的字段
        /// <summary>
        /// 质量
        /// </summary>
        protected double _mass;

        /// <summary>
        /// 质量的倒数
        /// </summary>
        protected double _inverseMass;

        /// <summary>
        /// 物体绑定的形状
        /// </summary>
        protected Shape _shape = new Point();

        /// <summary>
        /// 物体所受的力的合力
        /// </summary>
        protected Vector2D _forceAccum;
        #endregion

        #region 属性
        /// <summary>
        /// 为物体施加力
        /// </summary>
        /// <param name="force"></param>
        public void AddForce(Vector2D force)
        {
            _forceAccum += force;
        }

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

        public Shape Shape { get { return _shape; } }
        #endregion

        #region 公开的方法
        /// <summary>
        /// 为物体绑定一个形状
        /// </summary>
        /// <param name="shape"></param>
        public void BindShape(Shape shape)
        {
            shape.Body = this;
            _shape = shape;
        }

        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="duration"></param>
        public abstract void Update(double duration);
        #endregion
    }
}