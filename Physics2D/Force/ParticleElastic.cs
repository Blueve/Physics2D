namespace Physics2D.Force
{
    using System.Collections.Generic;
    using Physics2D.Object;

    public class ParticleElastic : ParticleForceGenerator
    {
        private readonly List<Particle> linked = new List<Particle>();

        private readonly double k;
        private readonly double length;

        public ParticleElastic(double k, double length)
        {
            this.k = k;
            this.length = length;
        }

        public void LinkWith(Particle item)
        {
            this.linked.Add(item);
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            foreach (var item in this.linked)
            {
                var d = particle.Position - item.Position;

                double force = (this.length - d.Length()) * this.k;
                d.Normalize();
                particle.AddForce(d * force);
            }
        }
    }
}