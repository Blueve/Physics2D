using Physics2D.Force;
using Physics2D.Object;
using System.Collections.Generic;

namespace Physics2D.Core
{
    /// <summary>
    /// 粒子的作用力发生器管理模块
    /// </summary>
    public sealed class ForceRegistry
    {
        #region 私有部分
        /// <summary>
        /// 作用力发生器集合
        /// </summary>
        private readonly HashSet<ParticleForceGenerator> _generators = new HashSet<ParticleForceGenerator>();
        #endregion

        #region 物体管理
        /// <summary>
        /// 添加一个新项目
        /// </summary>
        /// <param name="forceGenerator">作用力发生器</param>
        public void Add(ParticleForceGenerator forceGenerator) => _generators.Add(forceGenerator);

        /// <summary>
        /// 删除一个作用力发生器
        /// </summary>
        /// <param name="forceGenerator">作用力发生器</param>
        public void Remove(ParticleForceGenerator forceGenerator) => _generators.Remove(forceGenerator);

        /// <summary>
        /// 删除一个项目
        /// 依据粒子进行删除，只要包含该粒子，即执行删除操作
        /// </summary>
        /// <param name="particle">粒子</param>
        public void Remove(Particle particle)
        {
            foreach (var particleForceGenerator in _generators)
            {
                particleForceGenerator.Remove(particle);
            }
        }

        #endregion

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
        }
        #endregion
    }
}