using Physics2D.Object;

namespace Physics2D.Collision.Basic
{
    public abstract class ParticleLink : ParticleContactGenerator
    {
        /// <summary>
        /// 质体A
        /// </summary>
        public Particle ParticleA;
        /// <summary>
        /// 质体B
        /// </summary>
        public Particle ParticleB;

        /// <summary>
        /// 返回当前连接的长度
        /// </summary>
        /// <returns></returns>
        protected double CurrentLength()
        {
            return (ParticleA.Position - ParticleB.Position).Length();
        }
    }
}
