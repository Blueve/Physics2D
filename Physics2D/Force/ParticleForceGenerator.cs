using System.Collections.Generic;
using Physics2D.Object;

namespace Physics2D.Force
{
    public abstract class ParticleForceGenerator
    {
        /// <summary>
        /// 受管理的质体集合
        /// </summary>
        private readonly HashSet<Particle> _objects = new HashSet<Particle>();

        /// <summary>
        /// 添加新的受力对象
        /// </summary>
        /// <param name="particle">受力对象</param>
        public void Add(Particle particle) => _objects.Add(particle);

        /// <summary>
        /// 移除受力对象
        /// </summary>
        /// <param name="particle">受力对象</param>
        public void Remove(Particle particle) => _objects.Remove(particle);

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
            foreach (var particle in _objects)
            {
                ApplyTo(particle, duration);
            }
        }
    }
}