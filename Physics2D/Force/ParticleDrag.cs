namespace Physics2D.Force
{
    using Physics2D.Common;
    using Physics2D.Object;

    public class ParticleDrag : ParticleForceGenerator
    {
        private readonly double k1;
        private readonly double k2;

        public ParticleDrag(double k1, double k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            if (particle.Velocity == Vector2D.Zero)
            {
                return;
            }

            double c = particle.Velocity.Length();
            c = this.k1 * c + this.k2 * c * c;

            particle.AddForce(particle.Velocity.Normalize() * -c);
        }
    }
}