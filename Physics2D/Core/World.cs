namespace Physics2D.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Force.Zones;
    using Physics2D.Object;
    using Physics2D.Object.Tools;

    public sealed class World
    {
        /// <summary>
        /// 物体集合
        /// </summary>
        private readonly HashSet<PhysicsObject> objects;

        /// <summary>
        /// 边缘集合
        /// </summary>
        private readonly HashSet<Edge> edges;

        /// <summary>
        /// Pin集合
        /// </summary>
        private readonly Dictionary<IPin, Handle> pins = new Dictionary<IPin, Handle>();

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

        public World()
        {
            this.objects = new HashSet<PhysicsObject>();
            this.edges = new HashSet<Edge>();
            this.ContactGenerators = new ContactRegistry(this.objects, this.edges);
        }

        /// <summary>
        /// 向物理世界中添加一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(PhysicsObject obj)
        {
            this.objects.Add(obj);
        }

        /// <summary>
        /// 从物理世界中移除一个物体
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(PhysicsObject obj)
        {
            this.objects.Remove(obj);

            // 仅在物体为质体时执行注销操作
            var particle = obj as Particle;
            if (particle != null)
            {
                this.ForceGenerators.Remove(particle);
            }
        }

        /// <summary>
        /// 向物理世界添加一个定制的物体
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(CustomObject obj)
        {
            this.AddObject((PhysicsObject)obj);
            obj.OnInit(this);
        }

        /// <summary>
        /// 从物理世界移除一个定制的物体
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(CustomObject obj)
        {
            this.RemoveObject((PhysicsObject)obj);
            obj.OnRemove(this);
        }

        /// <summary>
        /// 向物理世界添加一条边
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            this.edges.Add(edge);
        }

        /// <summary>
        /// 从物理世界移除一条边
        /// </summary>
        /// <param name="edge"></param>
        public void RemoveEdge(Edge edge)
        {
            this.edges.Remove(edge);
        }

        /// <summary>
        /// 将制定物体Pin在物理世界
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Handle Pin(IPin obj, Vector2D position)
        {
            if (!this.pins.ContainsKey(obj))
            {
                this.pins[obj] = obj.Pin(this, position);
            }
            else
            {
                throw new InvalidOperationException("Can't pin target object which was alreadly pinned.");
            }

            return this.pins[obj];
        }

        /// <summary>
        /// 解除指定物体在物理世界的Pin
        /// </summary>
        /// <param name="obj"></param>
        public void UnPin(IPin obj)
        {
            if (this.pins.ContainsKey(obj))
            {
                obj.Unpin(this);
                this.pins[obj].Release();
                this.pins.Remove(obj);
            }
            else
            {
                throw new InvalidOperationException("Can't unpin target object which was not pinned.");
            }
        }

        /// <summary>
        /// 按时间间隔更新整个物理世界
        /// </summary>
        /// <param name="duration">时间间隔</param>
        public void Update(double duration)
        {
            // 为粒子施加作用力
            this.ForceGenerators.Update(duration);

            // 更新物理对象
            Parallel.ForEach(this.objects, item =>
            {
                // 为物理对象施加区域作用力
                foreach (var z in this.Zones)
                {
                    z.TryApplyTo(item, duration);
                }

                // 对物理对象进行积分
                item.Update(duration);
            });

            // 质体碰撞检测
            this.ContactGenerators.ResolveContacts(duration);
        }
    }
}