using Physics2D.Common;

namespace Physics2D.Force
{
    public class ParticleGravity : ParticleForceGenerator
    {
        private Vector2D gravity;

        public ParticleGravity(Vector2D gravity)
        {
            this.gravity = gravity;
        }

        public override void UpdateForce(Object.Particle particle, float duration)
        {
            // 质量无限大的物体不受重力影响
            if (particle.InverseMass == 0f) return;

            // 施加重力
            particle.AddForce(gravity * particle.Mass);
        }
    }
}