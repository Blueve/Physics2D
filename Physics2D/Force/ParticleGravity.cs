namespace Physics2D.Force
{
    using Physics2D.Common;
    using Physics2D.Object;

    public class ParticleGravity : ParticleForceGenerator
    {
        private readonly Vector2D gravity;

        public ParticleGravity(Vector2D gravity)
        {
            this.gravity = gravity;
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            // 质量无限大的物体不受重力影响
            if (particle.InverseMass == 0)
            {
                return;
            }

            // 施加重力
            particle.AddForce(this.gravity * particle.Mass);
        }
    }
}