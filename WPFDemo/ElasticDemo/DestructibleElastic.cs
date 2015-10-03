using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Object;
using System.Collections.Generic;

namespace WPFDemo.ElasticDemo
{
    internal class DestructibleElastic : ParticleForceGenerator
    {
        private readonly List<LinkedItem> _linked = new List<LinkedItem>();

        private readonly double _k;
        private readonly double _length;

        public DestructibleElastic(double k, double length)
        {
            _k = k;
            _length = length;
        }

        public void Joint(Particle item)
        {
            _linked.Add(new LinkedItem
            {
                Particle = item,
                IsValid = true
            });
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            for (int i = _linked.Count - 1; i >= 0; i--)
            {
                if (_linked[i].IsValid)
                {
                    var d = particle.Position - _linked[i].Particle.Position;
                    if (d.Length() > 1.5)
                    {
                        _linked[i].IsValid = false;
                        continue;
                    }
                    double force = (_length - d.Length()) * _k;
                    particle.AddForce(d.Normalize() * force);
                }
            }
        }

        private class LinkedItem
        {
            public Particle Particle;
            public bool IsValid;
        }
    }
}