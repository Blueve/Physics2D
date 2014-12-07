using Physics2D.Object;

namespace Physics2D.Force
{
    public abstract class ParticleForceGenerator
    {
        public abstract void UpdateForce(Particle particle, float duration);
    }
}