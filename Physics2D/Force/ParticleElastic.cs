using Physics2D.Common;
using Physics2D.Object;
using System.Collections.Generic;

namespace Physics2D.Force
{
    public class ParticleElastic : ParticleForceGenerator
    {
        private List<Particle> linked = new List<Particle>();

        private float k;
        private float length;

        public ParticleElastic(float k, float length)
        {
            this.k = k;
            this.length = length;
        }

        public void Add(Particle item)
        {
            linked.Add(item);
        }

        public override void UpdateForce(Particle particle, float duration)
        {
            foreach (var item in linked)
            {
                Vector2D d = particle.Position - item.Position;

                float force = (length - d.Length()) * k;
                d.Normalize();
                particle.AddForce(d * force);
            }
        }
    }
}