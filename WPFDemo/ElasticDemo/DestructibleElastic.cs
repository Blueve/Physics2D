using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Object;
using System.Collections.Generic;

namespace WPFDemo.ElasticDemo
{
    internal class DestructibleElastic : ParticleForceGenerator
    {
        private List<LinkedItem> linked = new List<LinkedItem>();

        private float k;
        private float length;

        public DestructibleElastic(float k, float length)
        {
            this.k = k;
            this.length = length;
        }

        public void Add(Particle item)
        {
            linked.Add(new LinkedItem
            {
                particle = item,
                isValid = true
            });
        }

        public override void UpdateForce(Particle particle, float duration)
        {
            for (int i = linked.Count - 1; i >= 0; i--)
            {
                if (linked[i].isValid)
                {
                    Vector2D d = particle.Position - linked[i].particle.Position;
                    if (d.Length() > 1.5f)
                    {
                        linked[i].isValid = false;
                        continue;
                    }
                    float force = (length - d.Length()) * k;
                    d.Normalize();
                    particle.AddForce(d * force);
                }
            }
        }

        private class LinkedItem
        {
            public Particle particle;
            public bool isValid;
        }
    }
}