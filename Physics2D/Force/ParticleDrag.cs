using Physics2D.Common;

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

        public override void UpdateForce(Object.Particle particle, double duration)
        {
            if (particle.Velocity == Vector2D.Zero) return;

            Vector2D force = particle.Velocity;
            double c = force.Length();
            c = _k1 * c + _k2 * c * c;
            force.Normalize();
            force *= -c;
            particle.AddForce(force);
        }
    }
}