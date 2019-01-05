namespace Physics2D.Object
{
    using System;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;

    public abstract class PhysicsObject
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
        /// 碰撞回弹系数
        /// </summary>
        public double Restitution = 1;

        /// <summary>
        /// 质量
        /// </summary>
        protected double mass;

        /// <summary>
        /// 质量的倒数
        /// </summary>
        protected double inverseMass;

        /// <summary>
        /// 物体绑定的形状
        /// </summary>
        protected Shape shape = new Point();

        /// <summary>
        /// 物体所受的力的合力
        /// </summary>
        protected Vector2D forceAccum;

        /// <summary>
        /// 为物体施加力
        /// </summary>
        /// <param name="force"></param>
        public void AddForce(Vector2D force)
        {
            this.forceAccum += force;
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
                    this.mass = value;
                    this.inverseMass = 1.0 / value;
                }
                else
                    throw new ArgumentOutOfRangeException("Particle's mass cannot be zero.");
            }
            get { return this.mass; }
        }

        /// <summary>
        /// 质量的倒数
        /// </summary>
        public double InverseMass
        {
            set
            {
                this.mass = value == 0 ? double.MaxValue : 1.0 / value;
                this.inverseMass = value;
            }
            get { return this.inverseMass; }
        }

        public Shape Shape { get { return this.shape; } }

        /// <summary>
        /// 为物体绑定一个形状
        /// </summary>
        /// <param name="shape"></param>
        public void BindShape(Shape shape)
        {
            shape.Body = this;
            this.shape = shape;
        }

        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="duration"></param>
        public abstract void Update(double duration);
    }
}