using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Collision
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
            PA = pA;
            PB = pB;
        }

        public override IEnumerator<ParticleContact> GetEnumerator()
        {
            double length = CurrentLength();

            // 未超过绳索长度
            if (length < MaxLength) yield break;

            var normal = (PB.Position - PA.Position).Normalize();

            yield return new ParticleContact
            {
                PA = PA,
                PB = PB,
                Restitution = Restitution,
                ContactNormal = normal,
                Penetration = length - MaxLength
            };
        }
    }
}
