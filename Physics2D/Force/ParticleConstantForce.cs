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
        private Vector2D force;

        public ParticleConstantForce(Vector2D _force)
        {
            force = _force;
        }

        public override void UpdateForce(Particle particle, float duration)
        {
            particle.AddForce(force);
        }
    }
}
