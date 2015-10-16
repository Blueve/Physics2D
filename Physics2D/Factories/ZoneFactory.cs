using Physics2D.Core;
using Physics2D.Force;
using Physics2D.Force.Zones;

namespace Physics2D.Factories
{
    /// <summary>
    /// 区域作用力工厂
    /// </summary>
    public static class ZoneFactory
    {
        #region 工厂方法

        /// <summary>
        /// 创建一个全局作用力区域
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="particleForceGenerator">作用力发生器</param>
        /// <returns></returns>
        public static GlobalZone CreateGlobalZone(this World world, ParticleForceGenerator particleForceGenerator)
        {
            var zone = new GlobalZone();
            RegistryZone(world, particleForceGenerator, zone);
            return zone;
        }

        /// <summary>
        /// 创建一个矩形区域作用力
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="particleForceGenerator">作用力发生器</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static RectangleZone CreateRectangleZone(this World world, ParticleForceGenerator particleForceGenerator, double x1, double y1, double x2, double y2)
        {
            var zone = new RectangleZone(x1, y1, x2, y2);
            RegistryZone(world, particleForceGenerator, zone);
            return zone;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 注册区域
        /// </summary>
        /// <param name="world">物理区域</param>
        /// <param name="particleForceGenerator">作用力发生器</param>
        /// <param name="zone">区域</param>
        private static void RegistryZone(this World world, ParticleForceGenerator particleForceGenerator, Zone zone)
        {
            zone.Add(particleForceGenerator);
            world.Zones.Add(zone);
        }

        #endregion
    }
}