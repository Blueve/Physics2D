using Physics2D.Common;
using Physics2D.Object;
using System.Collections.Generic;

namespace Physics2D.Force
{
    public class ParticleElastic : ParticleForceGenerator
    {
        private List<Particle> linked = new List<Particle>();

        private double k;
        private double length;

        public ParticleElastic(double k, double length)
        {
            this.k = k;
            this.length = length;
        }

        public void Add(Particle item)
        {
            linked.Add(item);
        }

        public override void UpdateForce(Particle particle, double duration)
        {
            foreach (var item in linked)
            {
                Vector2D d = particle.Position - item.Position;

                double force = (length - d.Length()) * k;
                d.Normalize();
                particle.AddForce(d * force);
            }
        }
    }
}