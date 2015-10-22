using Physics2D.Common;
using System;

namespace Physics2D.Object
{
    public class Particle : PhysicsObject
    {
        public bool IsTransparent = false;

        private Vector2D _forceAccum;

        public void AddForce(Vector2D force)
        {
            _forceAccum += force;
        }

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