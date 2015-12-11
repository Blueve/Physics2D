using Physics2D.Collision;
using Physics2D.Collision.Shapes;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Object;
using Physics2D.Object.Tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Physics2D.Core
{
    public sealed class World
    {
        #region 私有字段
        /// <summary>
        /// 物体集合
        /// </summary>
        private readonly HashSet<PhysicsObject> _objects;

        /// <summary>
        /// 边缘集合
        /// </summary>
        private readonly HashSet<Edge> _edges;

        /// <summary>
        /// Pin集合
        /// </summary>
        private readonly Dictionary<IPin, Handle> _pins = new Dictionary<IPin, Handle>();
        #endregion

        #region 只读字段
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
        public readonly ContactRegistry ContactGenerators;
        #endregion

        #region 构造方法
        public World()
        {
            _objects = new HashSet<PhysicsObject>();
            _edges = new HashSet<Edge>();
            ContactGenerators = new ContactRegistry(_objects, _edges);
        }
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
        /// 向物理世界添加一个定制的物体
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(CustomObject obj)
        {
            AddObject((PhysicsObject)obj);
            obj.OnInit(this);
        }

        /// <summary>
        /// 从物理世界移除一个定制的物体
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(CustomObject obj)
        {
            RemoveObject((PhysicsObject)obj);
            obj.OnRemove(this);
        }

        /// <summary>
        /// 向物理世界添加一条边
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
        }

        /// <summary>
        /// 从物理世界移除一条边
        /// </summary>
        /// <param name="edge"></param>
        public void RemoveEdge(Edge edge)
        {
            _edges.Remove(edge);
        }

        /// <summary>
        /// 将制定物体Pin在物理世界
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Handle Pin(IPin obj, Vector2D position)
        {
            if(!_pins.ContainsKey(obj))
            {
                _pins[obj] = obj.Pin(this, position);
            }
            else
            {
                throw new InvalidOperationException("Can't pin target object which was alreadly pinned.");
            }
            return _pins[obj];
        }

        /// <summary>
        /// 解除指定物体在物理世界的Pin
        /// </summary>
        /// <param name="obj"></param>
        public void UnPin(IPin obj)
        {
            if (_pins.ContainsKey(obj))
            {
                obj.UnPin(this);
                _pins[obj].Release();
                _pins.Remove(obj);
            }
            else
            {
                throw new InvalidOperationException("Can't unpin target object which was not pinned.");
            }
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