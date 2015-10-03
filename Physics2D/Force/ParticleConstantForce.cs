using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Force
{
    public class ParticleConstantForce : ParticleForceGenerator
    {
        private readonly Vector2D _force;

        public ParticleConstantForce(Vector2D force)
        {
            _force = force;
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            particle.AddForce(_force);
        }
    }
}
