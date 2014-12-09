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
        private List<PhysicsObject> objectSet = new List<PhysicsObject>();

        /// <summary>
        /// 质体作用力管理器
        /// </summary>
        private ParticleForceRegistry particleForceRegistry = new ParticleForceRegistry();

        /// <summary>
        /// 质体碰撞管理器
        /// </summary>
        private List<ParticleContactGenerator> particleContactRegistry = new List<ParticleContactGenerator>();

        /// <summary>
        /// 质体碰撞表
        /// </summary>
        private List<ParticleContact> particleContactList = new List<ParticleContact>();

        private ParticleContactResolver particleContactResolver = new ParticleContactResolver(0);
        #endregion 私有属性

        #region 只读属性

        /// <summary>
        /// 作用力区域集合
        /// </summary>
        public readonly List<Zone> ZoneSet = new List<Zone>();

        #endregion 只读属性

        #region 公开的管理方法

        /// <summary>
        /// 向物理世界中添加一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(PhysicsObject obj)
        {
            if (!objectSet.Contains(obj))
                objectSet.Add(obj);
        }

        /// <summary>
        /// 从物理世界中移除一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(PhysicsObject obj)
        {
            if (objectSet.Contains(obj))
                objectSet.Remove(obj);

            // 仅在物体为质体时执行注销操作
            if (obj is Particle)
            {
                particleForceRegistry.Remove((Particle)obj);
            }
        }

        /// <summary>
        /// 注册质体作用力发生器
        /// </summary>
        /// <param name="particle">质体</param>
        /// <param name="forceGenerator">作用力发生器</param>
        public void RegistryForceGenerator(Particle particle, ParticleForceGenerator forceGenerator)
        {
            particleForceRegistry.Add(particle, forceGenerator);
        }

        /// <summary>
        /// 注册质体碰撞发生器
        /// </summary>
        /// <param name="contactGenerator">碰撞发生器</param>
        public void RegistryContactGenerator(ParticleContactGenerator contactGenerator)
        {
            if (!particleContactRegistry.Contains(contactGenerator))
                particleContactRegistry.Add(contactGenerator);
        }
        #endregion 公开的管理方法

        #region 公开的方法

        /// <summary>
        /// 按时间间隔更新整个物理世界
        /// </summary>
        /// <param name="duration">时间间隔</param>
        public void Update(float duration)
        {
            // 为粒子施加作用力
            particleForceRegistry.Update(duration);

            // 更新物理对象
            Parallel.ForEach(objectSet, (item) =>
            {
                // 为物理对象施加区域作用力
                foreach (var zone in ZoneSet)
                {
                    zone.TryApplyTo(item, duration);
                }
                // 对物理对象进行积分
                item.Update(duration);
            });

            // 质体碰撞检测
            particleContactList.Clear();
            int limit = Settings.maxContacts;
            foreach(var item in particleContactRegistry)
            {
                int used = item.addContact(particleContactList, limit);
                limit -= used;

                if (limit <= 0) break;
            }
            // 解决质体碰撞
            particleContactResolver.Iterations = (Settings.maxContacts - limit) * 2;
            particleContactResolver.resolveContacts(particleContactList, duration);
        }

        #endregion 公开的方法

        #region 工厂方法

        /// <summary>
        /// 在物理世界中创建一个粒子
        /// </summary>
        /// <returns>粒子的引用</returns>
        public Particle CreateParticle(Vector2D p, Vector2D v, float m)
        {
            return ParticleFactory.Create(this, p, v, m);
        }

        #endregion 工厂方法
    }
}