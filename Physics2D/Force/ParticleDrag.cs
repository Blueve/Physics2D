using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Force
{
    public class ParticleDrag : ParticleForceGenerator
    {
        private readonly double _k1;
        private readonly double _k2;

        public ParticleDrag(double k1, double k2)
        {
            _k1 = k1;
            _k2 = k2;
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            if (particle.Velocity == Vector2D.Zero) return;
            
            double c = particle.Velocity.Length();
            c = _k1 * c + _k2 * c * c;
            
            particle.AddForce(particle.Velocity.Normalize() * -c);
        }
    }
}