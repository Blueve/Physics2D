using Physics2D.Common;
using Physics2D.Core;
using Physics2D.Object;

namespace Physics2D.Factories
{
    /// <summary>
    /// 粒子工厂
    /// </summary>
    public static class ParticleFactory
    {
        #region 工厂方法

        /// <summary>
        /// 创建一个粒子
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="p">初位置</param>
        /// <param name="v">初速度</param>
        /// <param name="m">质量</param>
        /// <returns></returns>
        public static Particle Create(World world, Vector2D p, Vector2D v, float m)
        {
            Particle particle = new Particle
            {
                Position = p,
                Mass = m,
                Velocity = v,
                PrePosition = p
            };
            world.AddObject(particle);
            return particle;
        }

        /// <summary>
        /// 创建固定不动的粒子
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="p">初位置</param>
        /// <returns></returns>
        public static Particle CreateFixed(World world, Vector2D p)
        {
            Particle particle = new Particle
            {
                Position = p,
                InverseMass = 0f,
                PrePosition = p
            };
            world.AddObject(particle);
            return particle;
        }

        /// <summary>
        /// 创建永远保持匀速运动的粒子
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="p">初位置</param>
        /// <param name="v">初速度</param>
        /// <returns></returns>
        public static Particle CreateFixed(World world, Vector2D p, Vector2D v)
        {
            Particle particle = new Particle
            {
                Position = p,
                InverseMass = 0f,
                Velocity = v,
                PrePosition = p
            };
            world.AddObject(particle);
            return particle;
        }

        #endregion 工厂方法
    }
}