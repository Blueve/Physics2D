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

        private List<ParticleForceRegistration> registrations = new List<ParticleForceRegistration>();

        private struct ParticleForceRegistration
        {
            public Particle particle;
            public ParticleForceGenerator forceGenerator;
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
            registrations.Add(new ParticleForceRegistration
            {
                particle = particle,
                forceGenerator = forceGenerator
            });
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
            registrations.RemoveAll(item =>
            {
                return item.particle == particle && item.forceGenerator == forceGenerator;
            });
        }

        /// <summary>
        /// 删除一个项目
        /// 依据粒子进行删除，只要包含该粒子，即执行删除操作
        /// </summary>
        /// <param name="particle">粒子</param>
        public void Remove(Particle particle)
        {
            registrations.RemoveAll(item =>
            {
                return item.particle == particle;
            });
        }

        #endregion 公开的管理方法

        #region 公开的方法

        /// <summary>
        /// 执行所有的作用力发生器
        /// </summary>
        /// <param name="durduration"></param>
        public void Update(double durduration)
        {
            registrations.ForEach(item => item.forceGenerator.UpdateForce(item.particle, durduration));
        }

        #endregion 公开的方法
    }
}