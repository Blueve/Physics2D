using System.Runtime.InteropServices.ComTypes;
using Physics2D.Common;
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
        /// 在物理世界创建一个全局作用力区域
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="particleForceGenerator">作用力发生器</param>
        /// <returns></returns>
        public static GlobalZone CreateGlobalZone(
            this World world, 
            ParticleForceGenerator particleForceGenerator)
        {
            var zone = new GlobalZone();
            return world.CreateZone(zone, particleForceGenerator);
        }

        /// <summary>
        /// 在物理世界创建一个矩形区域作用力
        /// </summary>
        /// <param name="world">物理世界</param>
        /// <param name="particleForceGenerator">作用力发生器</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static RectangleZone CreateRectangleZone(
            this World world,
            ParticleForceGenerator particleForceGenerator,
            double x1,
            double y1,
            double x2,
            double y2)
        {
            var zone = new RectangleZone(x1, y1, x2, y2);
            return world.CreateZone(zone, particleForceGenerator);
        }

        /// <summary>
        /// 为物理世界创建重力
        /// 重力的方向向下
        /// </summary>
        /// <param name="world"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static GlobalZone CreateGravity(this World world, double g)
        {
            return world.CreateGlobalZone(new ParticleGravity(new Vector2D(0, g)));
        }

        /// <summary>
        /// 在物理世界中创建一个区域
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <param name="zone"></param>
        /// <param name="particleForceGenerator"></param>
        /// <returns></returns>
        public static T CreateZone<T>(
            this World world, 
            T zone, 
            ParticleForceGenerator particleForceGenerator) 
            where T : Zone
        {
            zone.Add(particleForceGenerator);
            world.Zones.Add(zone);
            return zone;
        }

        #endregion
    }
}