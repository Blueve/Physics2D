using System.Collections.Generic;
using Physics2D.Object;

namespace Physics2D.Collision.Basic
{
    public class ParticleRope : ParticleLink
    {
        /// <summary>
        /// 最大长度
        /// </summary>
        public double MaxLength { get; }

        /// <summary>
        /// 弹性系数
        /// </summary>
        public double Restitution { get; }

        public ParticleRope(double maxLength, double restitution, Particle pA, Particle pB)
        {
            MaxLength = maxLength;
            Restitution = restitution;
            ParticleA = pA;
            ParticleB = pB;
        }

        public override IEnumerator<ParticleContact> GetEnumerator()
        {
            double length = CurrentLength();

            // 未超过绳索长度
            if (length < MaxLength) yield break;

            var normal = (ParticleB.Position - ParticleA.Position).Normalize();

            yield return new ParticleContact(ParticleA, ParticleB, Restitution, length - MaxLength, normal);
        }
    }
}
