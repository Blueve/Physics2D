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
                if (value != 0)
                {
                    mass = value;
                    inverseMass = 1f / value;
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            get { return mass; }
        }

        public double InverseMass
        {
            set
            {
                mass = value == 0 ? double.MaxValue : 1f / value;
                inverseMass = value;
            }
            get { return inverseMass; }
        }

        private double mass;
        private double inverseMass;
        private Vector2D forceAccum;

        public void AddForce(Vector2D force)
        {
            forceAccum += force;
        }

        public override void Update(double duration)
        {
            PrePosition = new Vector2D(Position);

            // 对位置速度以及加速度进行更新
            Acceleration = forceAccum * inverseMass;

            Position += Velocity * duration;
            Velocity += Acceleration * duration;

            // 清除作用力
            forceAccum = Vector2D.Zero;
        }
    }
}