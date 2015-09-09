using Physics2D.Common;
using System;

namespace Physics2D.Object
{
    public class Particle : PhysicsObject
    {
        public double Mass
        {
            set
            {
                if (value != .0)
                {
                    _mass = value;
                    _inverseMass = 1.0 / value;
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            get { return _mass; }
        }

        public double InverseMass
        {
            set
            {
                _mass = value == .0 ? double.MaxValue : 1.0 / value;
                _inverseMass = value;
            }
            get { return _inverseMass; }
        }

        private double _mass;
        private double _inverseMass;
        private Vector2D _forceAccum;

        public void AddForce(Vector2D force)
        {
            _forceAccum += force;
        }

        public override void Update(double duration)
        {
            PrePosition = new Vector2D(Position);

            // 对位置速度以及加速度进行更新
            Acceleration = _forceAccum * _inverseMass;

            Position += Velocity * duration;
            Velocity += Acceleration * duration;

            // 清除作用力
            _forceAccum = Vector2D.Zero;
        }
    }
}