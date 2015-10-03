using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Force
{
    public class ParticleGravity : ParticleForceGenerator
    {
        private readonly Vector2D _gravity;

        public ParticleGravity(Vector2D gravity)
        {
            _gravity = gravity;
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            // 质量无限大的物体不受重力影响
            if (particle.InverseMass == 0) return;

            // 施加重力
            particle.AddForce(_gravity * particle.Mass);
        }
    }
}