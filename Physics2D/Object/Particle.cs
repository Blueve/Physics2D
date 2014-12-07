using Physics2D.Common;
using System;

namespace Physics2D.Object
{
    public class Particle : PhysicsObject
    {
        public float Mass
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

        public float InverseMass
        {
            set
            {
                mass = value == 0 ? float.MaxValue : 1f / value;
                inverseMass = value;
            }
            get { return inverseMass; }
        }

        private float mass;
        private float inverseMass;
        private Vector2D forceAccum;

        public void AddForce(Vector2D force)
        {
            forceAccum += force;
        }

        public override void Update(float duration)
        {
            // 对位置和速度进行更新
            Position += Velocity * duration;
            Velocity += (Acceleration + forceAccum * InverseMass) * duration;

            // 清除作用力
            forceAccum = Vector2D.Zero;
        }
    }
}