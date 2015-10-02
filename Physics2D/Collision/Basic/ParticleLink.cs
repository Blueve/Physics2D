using Physics2D.Object;

namespace Physics2D.Collision.Basic
{
    public abstract class ParticleLink : ParticleContactGenerator
    {
        /// <summary>
        /// 质体A
        /// </summary>
        public Particle PA;
        /// <summary>
        /// 质体B
        /// </summary>
        public Particle PB;

        /// <summary>
        /// 返回当前连接的长度
        /// </summary>
        /// <returns></returns>
        protected double CurrentLength()
        {
            return (PA.Position - PB.Position).Length();
        }
    }
}
