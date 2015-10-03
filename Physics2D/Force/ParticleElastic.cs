using Physics2D.Common;
using Physics2D.Object;
using System.Collections.Generic;

namespace Physics2D.Force
{
    public class ParticleElastic : ParticleForceGenerator
    {
        private readonly List<Particle> _linked = new List<Particle>();

        private readonly double _k;
        private readonly double _length;

        public ParticleElastic(double k, double length)
        {
            _k = k;
            _length = length;
        }

        public void Add(Particle item)
        {
            _linked.Add(item);
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            foreach (var item in _linked)
            {
                var d = particle.Position - item.Position;

                double force = (_length - d.Length()) * _k;
                d.Normalize();
                particle.AddForce(d * force);
            }
        }
    }
}