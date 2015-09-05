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
        public double maxLength;

        /// <summary>
        /// 弹性系数
        /// </summary>
        public double restitution;

        public ParticleRope(double maxLength, double restitution, Particle pA, Particle pB)
        {
            this.maxLength = maxLength;
            this.restitution = restitution;
            PA = pA;
            PB = pB;
        }

        public override int fillContact(List<ParticleContact> contactList, int limit)
        {
            double length = currentLength();

            // 未超过绳索长度
            if (length < maxLength) return 0;

            Vector2D normal = (PB.Position -　PA.Position).Normalize();

            ParticleContact contact = new ParticleContact
            {
                PA = PA,
                PB = PB,
                restitution = restitution,
                contactNormal = normal,
                penetration = length - maxLength
            };

            contactList.Add(contact);

            return 1;
        }
    }
}
