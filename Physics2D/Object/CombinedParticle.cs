namespace Physics2D.Object
{
    using System.Collections.Generic;
    using Physics2D.Collision.Basic;
    using Physics2D.Common;
    using Physics2D.Common.Exceptions;
    using Physics2D.Core;
    using Physics2D.Factories;
    using Physics2D.Object.Tools;

    /// <summary>
    /// 由多个质点组合成的物体
    /// 质点由刚性连杆连接，可获得类似刚体的自由度
    /// </summary>
    public class CombinedParticle : CustomObject, IPin
    {
        /// <summary>
        /// 质体顶点表（顺时针）
        /// </summary>
        private readonly List<Particle> vertexs = new List<Particle>();

        /// <summary>
        /// 刚性连杆表（顺时针）
        /// </summary>
        private readonly List<ParticleRod> rods = new List<ParticleRod>();

        /// <summary>
        /// 锚点连杆表
        /// </summary>
        private List<ParticleRod> pinRods = new List<ParticleRod>();

        /// <summary>
        /// 是否封闭（首尾相连）
        /// </summary>
        private readonly bool isClose;

        /// <summary>
        /// 质体顶点表（顺时针）
        /// </summary>
        public IReadOnlyList<Particle> Vertexs => this.vertexs;

        /// <summary>
        /// 刚性连杆表（顺时针）
        /// </summary>
        public IReadOnlyList<ParticleRod> Rods => this.rods;

        /// <summary>
        /// 是否封闭（首尾相连）
        /// </summary>
        public bool IsClose => this.isClose;

        public IReadOnlyList<ParticleRod> PinRods => this.pinRods;

        /// <summary>
        /// 创建一个联合质体
        /// </summary>
        /// <param name="vertexs"></param>
        /// <param name="velocity"></param>
        /// <param name="mass"></param>
        /// <param name="restitution"></param>
        /// <param name="isClose"></param>
        public CombinedParticle(List<Vector2D> vertexs, double mass = 1, double restitution = 1, bool isClose = true)
        {
            var num = vertexs.Count;
            if (num < 2)
            {
                throw new InvalidArgumentException(
                    $"Can't create a combined particle by given vertexs. vertexs.Count = {vertexs.Count}", nameof(vertexs));
            }
            else if (isClose && num < 3)
            {
                throw new InvalidArgumentException(
                    $"Can't create a closed combined particle by given vertexs. vertexs.Count = {vertexs.Count}", nameof(vertexs));
            }

            foreach (var vertex in vertexs)
            {
                this.vertexs.Add(new Particle
                {
                    Position = vertex,
                    Mass = mass / num,
                    Restitution = restitution
                });
            }

            // 在可形成多边形的时候允许链接为封闭图形
            this.isClose = isClose;
            if (isClose && num > 2)
            {
                this.vertexs.Add(this.vertexs[0]);
            }

            // 创建刚性连杆
            for (int i = 1; i < this.vertexs.Count; i++)
            {
                this.rods.Add(new ParticleRod(this.vertexs[i - 1], this.vertexs[i]));
            }
        }

        /// <summary>
        /// 装载到物理世界
        /// </summary>
        /// <param name="world"></param>
        public override void OnInit(World world)
        {
            foreach (var vertex in this.vertexs)
            {
                world.AddObject(vertex);
            }

            foreach (var rod in this.rods)
            {
                world.ContactGenerators.Add(rod);
            }
        }

        /// <summary>
        /// 从物理世界中移除
        /// </summary>
        /// <param name="world"></param>
        public override void OnRemove(World world)
        {
            foreach (var vertex in this.vertexs)
            {
                world.RemoveObject(vertex);
            }
        }

        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="duration"></param>
        public override void Update(double duration)
        {
            // 组织质体的位移被视为所有质体的偏移量
            // 该偏移量在每次施加偏移过后置0
            var N = this.isClose ? this.vertexs.Count - 1 : this.vertexs.Count;
            for (int i = 0; i < N; i++)
            {
                this.vertexs[i].Position += this.Position;
            }

            this.Position = Vector2D.Zero;
        }

        Handle IPin.Pin(World world, Vector2D position)
        {
            return this.Pin(world, position);
        }

        void IPin.Unpin(World world)
        {
            this.UnPin(world);
        }

        protected Handle Pin(World world, Vector2D position)
        {
            var pin = new Particle
            {
                Position = position,
                InverseMass = 0
            };

            // 对于封闭图形，任意连接三个点即可固定住形状
            // 对于不封闭图形，需要每个点都连接才可固定
            var N = this.isClose ? 3 : this.vertexs.Count;
            for (int i = 0; i < N; i++)
            {
                this.pinRods.Add(world.CreateRod(this.vertexs[i], pin));
            }

            var handle = new Handle(position);
            handle.PropertyChanged += (obj, e) =>
            {
                var p = ((Handle)obj).Position;
                var d = p - pin.Position;
                this.Position = d;
                pin.Position = p;
            };

            return handle;
        }

        protected void UnPin(World world)
        {
            // 移除连接
            foreach (var rod in this.pinRods)
            {
                world.ContactGenerators.Remove(rod);
            }

            this.pinRods.Clear();

            // 恢复速度
            foreach (var vertex in this.vertexs)
            {
                vertex.Velocity = this.Velocity;
            }
        }
    }
}
