using Physics2D.Force;
using Physics2D.Object;
using System.Collections.Generic;

namespace Physics2D.Core
{
    /// <summary>
    /// 粒子的作用力发生器管理模块
    /// </summary>
    internal class ParticleForceRegistry
    {
        #region 私有部分

        private readonly HashSet<ParticleForceGenerator> _generators = new HashSet<ParticleForceGenerator>(); 

        private readonly List<ParticleForceRegistration> _registrations = new List<ParticleForceRegistration>();

        private struct ParticleForceRegistration
        {
            public Particle Particle;
            public ParticleForceGenerator ForceGenerator;
        }

        #endregion 私有部分

        #region 公开的管理方法

        /// <summary>
        /// 添加一个新项目
        /// </summary>
        /// <param name="particle">粒子</param>
        /// <param name="forceGenerator">作用力发生器</param>
        public void Add(Particle particle, ParticleForceGenerator forceGenerator)
        {
            //_registrations.Add(new ParticleForceRegistration
            //{
            //    Particle = particle,
            //    ForceGenerator = forceGenerator
            //});
            _generators.Add(forceGenerator);
            forceGenerator.Add(particle);
        }

        /// <summary>
        /// 删除一个项目
        /// 依据粒子和作用力发生器进行删除，两项均满足的时候才会执行
        /// 删除操作
        /// </summary>
        /// <param name="particle">粒子</param>
        /// <param name="forceGenerator">作用力发生器</param>
        public void Remove(Particle particle, ParticleForceGenerator forceGenerator)
        {
            //_registrations.RemoveAll(item => item.Particle == particle && item.ForceGenerator == forceGenerator);
            forceGenerator.Remove(particle);
        }

        /// <summary>
        /// 删除一个项目
        /// 依据粒子进行删除，只要包含该粒子，即执行删除操作
        /// </summary>
        /// <param name="particle">粒子</param>
        public void Remove(Particle particle)
        {
            //_registrations.RemoveAll(item => item.Particle == particle);
            foreach (var particleForceGenerator in _generators)
            {
                particleForceGenerator.Remove(particle);
            }
        }

        #endregion 公开的管理方法

        #region 公开的方法

        /// <summary>
        /// 执行所有的作用力发生器
        /// </summary>
        /// <param name="duration"></param>
        public void Update(double duration)
        {
            foreach (var particleForceGenerator in _generators)
            {
                particleForceGenerator.Apply(duration);
            }
            //_registrations.ForEach(item => item.ForceGenerator.ApplyTo(item.Particle, duration));
        }

        #endregion 公开的方法
    }
}