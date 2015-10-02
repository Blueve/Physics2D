using Physics2D.Collision;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Physics2D.Core
{
    sealed public class World
    {
        #region 私有属性
        /// <summary>
        /// 物体集合
        /// </summary>
        private readonly List<PhysicsObject> _objectRegistry = new List<PhysicsObject>();

        /// <summary>
        /// 质体作用力管理器
        /// </summary>
        private readonly ParticleForceRegistry _particleForceRegistry = new ParticleForceRegistry();

        /// <summary>
        /// 质体碰撞管理器
        /// </summary>
        private readonly ParticleContactRegistry _particleContactRegistry = new ParticleContactRegistry();
        #endregion 私有属性

        #region 只读属性
        /// <summary>
        /// 作用力区域集合
        /// </summary>
        public readonly List<Zone> ZoneRegistry = new List<Zone>();
        #endregion 只读属性

        #region 公开的管理方法
        /// <summary>
        /// 向物理世界中添加一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(PhysicsObject obj)
        {
            if (!_objectRegistry.Contains(obj))
                _objectRegistry.Add(obj);
        }

        /// <summary>
        /// 同AddObject方法
        /// 但需要注意的是，单独使用+运算符时的效果和
        /// 使用+=的效果时一致的，例如：
        /// 1) world + obj;
        /// 2) world += obj;
        /// world的值都会被更改，但建议使用第二种方式，语义会更加明确
        /// </summary>
        /// <param name="world"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static World operator +(World world, PhysicsObject obj)
        {
            world.AddObject(obj);
            return world;
        }

        /// <summary>
        /// 从物理世界中移除一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(PhysicsObject obj)
        {
            if (_objectRegistry.Contains(obj))
                _objectRegistry.Remove(obj);

            // 仅在物体为质体时执行注销操作
            var particle = obj as Particle;
            if (particle != null)
            {
                _particleForceRegistry.Remove(particle);
            }
        }

        /// <summary>
        /// 同RemoveObject方法
        /// </summary>
        /// <param name="world"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static World operator -(World world, PhysicsObject obj)
        {
            world.RemoveObject(obj);
            return world;
        }

        /// <summary>
        /// 注册质体作用力发生器
        /// </summary>
        /// <param name="particle">质体</param>
        /// <param name="forceGenerator">作用力发生器</param>
        public void RegistryForceGenerator(Particle particle, ParticleForceGenerator forceGenerator)
        {
            _particleForceRegistry.Add(particle, forceGenerator);
        }

        /// <summary>
        /// 注册质体碰撞发生器
        /// </summary>
        /// <param name="contactGenerator">碰撞发生器</param>
        public void RegistryContactGenerator(ParticleContactGenerator contactGenerator)
        {
            _particleContactRegistry.Add(contactGenerator);
        }
        #endregion 公开的管理方法

        #region 公开的方法
        /// <summary>
        /// 按时间间隔更新整个物理世界
        /// </summary>
        /// <param name="duration">时间间隔</param>
        public void Update(double duration)
        {
            // 为粒子施加作用力
            _particleForceRegistry.Update(duration);

            // 更新物理对象
            Parallel.ForEach(_objectRegistry, item =>
            {
                // 为物理对象施加区域作用力
                ZoneRegistry.ForEach(z => z.TryApplyTo(item, duration));
                // 对物理对象进行积分
                item.Update(duration);
            });

            // 质体碰撞检测
            _particleContactRegistry.ResolveContacts(duration);
        }
        #endregion 公开的方法
    }
}