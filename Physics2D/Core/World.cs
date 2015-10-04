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
        private readonly HashSet<PhysicsObject> _objects = new HashSet<PhysicsObject>();
        #endregion

        #region 只读属性
        /// <summary>
        /// 作用力区域集合
        /// </summary>
        public readonly HashSet<Zone> Zones = new HashSet<Zone>();

        /// <summary>
        /// 质体作用力管理器
        /// </summary>
        public readonly ForceRegistry ForceGenerators = new ForceRegistry();

        /// <summary>
        /// 质体碰撞管理器
        /// </summary>
        public readonly ContactRegistry ContactGenerators = new ContactRegistry();
        #endregion

        #region 物体管理
        /// <summary>
        /// 向物理世界中添加一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(PhysicsObject obj)
        {
            _objects.Add(obj);
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
            _objects.Remove(obj);

            // 仅在物体为质体时执行注销操作
            var particle = obj as Particle;
            if (particle != null)
            {
                ForceGenerators.Remove(particle);
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
        #endregion

        #region 公开方法
        /// <summary>
        /// 按时间间隔更新整个物理世界
        /// </summary>
        /// <param name="duration">时间间隔</param>
        public void Update(double duration)
        {
            // 为粒子施加作用力
            ForceGenerators.Update(duration);

            // 更新物理对象
            Parallel.ForEach(_objects, item =>
            {
                // 为物理对象施加区域作用力
                foreach (var z in Zones)
                {
                    z.TryApplyTo(item, duration);
                }
                // 对物理对象进行积分
                item.Update(duration);
            });

            // 质体碰撞检测
            ContactGenerators.ResolveContacts(duration);
        }
        #endregion
    }
}