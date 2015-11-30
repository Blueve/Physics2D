using Physics2D.Common;
using System;

namespace Physics2D.Object
{
    public class Particle : PhysicsObject
    {
        /// <summary>
        /// 更新质体
        /// </summary>
        /// <param name="duration"></param>
        public override void Update(double duration)
        {
            PrePosition = Position;

            // 对位置速度以及加速度进行更新
            Acceleration = _forceAccum * _inverseMass;

            Position += Velocity * duration;
            Velocity += Acceleration * duration;

            // 清除作用力
            _forceAccum = Vector2D.Zero;
        }
    }
}