namespace Physics2D.Collision.Basic
{
    using System.Collections.Generic;
    using Physics2D.Object;

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
            this.MaxLength = maxLength;
            this.Restitution = restitution;
            this.ParticleA = pA;
            this.ParticleB = pB;
        }

        public override IEnumerator<ParticleContact> GetEnumerator()
        {
            double length = this.CurrentLength();

            // 未超过绳索长度
            if (length < this.MaxLength)
            {
                yield break;
            }

            var normal = (this.ParticleB.Position - this.ParticleA.Position).Normalize();

            yield return new ParticleContact(this.ParticleA, this.ParticleB, this.Restitution, length - this.MaxLength, normal);
        }
    }
}
