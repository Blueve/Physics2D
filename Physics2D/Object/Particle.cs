using Physics2D.Common;
using System;

namespace Physics2D.Object
{
    public class Particle : PhysicsObject
    {
        /// <summary>
        /// 是否可穿透
        /// 若一个质体被设置为可穿透，那它将不会和其它质体碰撞
        /// </summary>
        public bool IsTransparent = false;

        /// <summary>
        /// 更新质体
        /// </summary>
        /// <param name="duration"></param>
        public override void Update(double duration)
        {
            PrePosition = Position;

            // 对位置速度以及加速度进行更新
            Acceleration = _forceAccum * _inverseMass;
            Velocity += Acceleration * duration;
            Position += Velocity * duration;
            

            // 清除作用力
            _forceAccum = Vector2D.Zero;
        }
    }
}