using Physics2D.Object;
using System.Collections.Generic;

namespace Physics2D.Force.Zones
{
    /// <summary>
    /// 作用力区域
    /// 使在区域内的粒子均受到指定的作用力
    /// </summary>
    public abstract class Zone
    {
        /// <summary>
        /// 区域内粒子作用力发生器
        /// </summary>
        public readonly List<ParticleForceGenerator> particleForceGenerators = new List<ParticleForceGenerator>();

        /// <summary>
        /// 判断给定物体是否存在于当前区域
        /// </summary>
        /// <param name="obj">物体</param>
        /// <returns></returns>
        protected abstract bool isIn(PhysicsObject obj);

        /// <summary>
        /// 尝试为给定物体施加作用力
        /// </summary>
        /// <param name="obj">给定物体</param>
        /// <param name="durduration">施加作用力的时间</param>
        public void TryApplyTo(PhysicsObject obj, float durduration)
        {
            if (isIn(obj))
            {
                foreach (var item in particleForceGenerators)
                {
                    if (obj is Particle)
                        item.UpdateForce((Particle)obj, durduration);
                }
            }
        }
    }
}