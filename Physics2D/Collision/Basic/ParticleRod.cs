using System.Collections.Generic;
using Physics2D.Object;

namespace Physics2D.Collision.Basic
{
    public class ParticleRod : ParticleLink
    {
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; }

        public ParticleRod(Particle pA, Particle pB)
        {
            Length = (pB.Position - pA.Position).Length();
            ParticleA = pA;
            ParticleB = pB;
        }


        public override IEnumerator<ParticleContact> GetEnumerator()
        {
            double length = CurrentLength();
            double penetration = length - Length;

            if (penetration == 0) yield break;

            var normal = (ParticleB.Position - ParticleA.Position).Normalize();

            if (length <= Length)
            {
                normal *= -1;
                penetration *= -1;
            }

            yield return new ParticleContact(ParticleA, ParticleB, 0, penetration, normal);
        }
    }
}
