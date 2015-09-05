using Physics2D.Common;

namespace Physics2D.Force
{
    public class ParticleDrag : ParticleForceGenerator
    {
        private double k1;
        private double k2;

        public ParticleDrag(double k1, double k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        public override void UpdateForce(Object.Particle particle, double duration)
        {
            if (particle.Velocity == Vector2D.Zero) return;

            Vector2D force = particle.Velocity;
            double c = force.Length();
            c = k1 * c + k2 * c * c;
            force.Normalize();
            force *= -c;
            particle.AddForce(force);
        }
    }
}