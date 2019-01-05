namespace Physics2D.Force
{
    using System.Collections.Generic;
    using Physics2D.Object;

    public abstract class ParticleForceGenerator
    {
        /// <summary>
        /// 受管理的质体集合
        /// </summary>
        protected readonly HashSet<Particle> Objects = new HashSet<Particle>();

        /// <summary>
        /// 添加新的受力对象
        /// </summary>
        /// <param name="particle">受力对象</param>
        public void Add(Particle particle) => this.Objects.Add(particle);

        /// <summary>
        /// 移除受力对象
        /// </summary>
        /// <param name="particle">受力对象</param>
        public void Remove(Particle particle) => this.Objects.Remove(particle);

        /// <summary>
        /// 为指定质体施加作用力
        /// </summary>
        /// <param name="particle">受力对象</param>
        /// <param name="duration"></param>
        public abstract void ApplyTo(Particle particle, double duration);

        /// <summary>
        /// 为所管理的所有对象施加作用力
        /// </summary>
        /// <param name="duration"></param>
        public void Apply(double duration)
        {
            foreach (var particle in this.Objects)
            {
                this.ApplyTo(particle, duration);
            }
        }
    }
}