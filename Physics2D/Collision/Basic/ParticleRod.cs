namespace Physics2D.Collision.Basic
{
    using System.Collections.Generic;
    using Physics2D.Object;

    public class ParticleRod : ParticleLink
    {
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; }

        public ParticleRod(Particle pA, Particle pB)
        {
            this.Length = (pB.Position - pA.Position).Length();
            this.ParticleA = pA;
            this.ParticleB = pB;
        }

        public override IEnumerator<ParticleContact> GetEnumerator()
        {
            double length = this.CurrentLength();
            double penetration = length - this.Length;

            if (penetration == 0)
            {
                yield break;
            }

            var normal = (this.ParticleB.Position - this.ParticleA.Position).Normalize();

            if (length <= this.Length)
            {
                normal *= -1;
                penetration *= -1;
            }

            yield return new ParticleContact(this.ParticleA, this.ParticleB, 0, penetration, normal);
        }
    }
}
