namespace Physics2D.Force.Zones
{
    using System.Collections.Generic;
    using Physics2D.Object;

    /// <summary>
    /// 作用力区域
    /// 使在区域内的粒子均受到指定的作用力
    /// </summary>
    public abstract class Zone
    {
        /// <summary>
        /// 区域内粒子作用力发生器
        /// </summary>
        private readonly HashSet<ParticleForceGenerator> particleForceGenerators = new HashSet<ParticleForceGenerator>();

        /// <summary>
        /// 添加一个作用力发生器
        /// </summary>
        /// <param name="particleForceGenerator"></param>
        public void Add(ParticleForceGenerator particleForceGenerator)
            => this.particleForceGenerators.Add(particleForceGenerator);

        /// <summary>
        /// 移除一个作用力发生器
        /// </summary>
        /// <param name="particleForceGenerator"></param>
        public void Remove(ParticleForceGenerator particleForceGenerator)
            => this.particleForceGenerators.Remove(particleForceGenerator);

        /// <summary>
        /// 判断给定物体是否存在于当前区域
        /// </summary>
        /// <param name="obj">物体</param>
        /// <returns></returns>
        protected abstract bool IsIn(PhysicsObject obj);

        /// <summary>
        /// 尝试为给定物体施加作用力
        /// </summary>
        /// <param name="obj">给定物体</param>
        /// <param name="duration">施加作用力的时间</param>
        public void TryApplyTo(PhysicsObject obj, double duration)
        {
            if (!this.IsIn(obj))
            {
                return;
            }

            if (obj is Particle)
            {
                foreach (var item in this.particleForceGenerators)
                {
                    item.ApplyTo((Particle)obj, duration);
                }
            }
        }
    }
}