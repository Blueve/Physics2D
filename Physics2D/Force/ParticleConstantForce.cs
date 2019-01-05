namespace Physics2D.Force
{
    using Physics2D.Common;
    using Physics2D.Object;

    public class ParticleConstantForce : ParticleForceGenerator
    {
        private readonly Vector2D force;

        public ParticleConstantForce(Vector2D force)
        {
            this.force = force;
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            particle.AddForce(this.force);
        }
    }
}
