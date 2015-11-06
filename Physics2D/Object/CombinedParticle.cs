using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Core;
using Physics2D.Collision.Shapes;
using Physics2D.Collision.Basic;
using Physics2D.Factories;
using Physics2D.Common;
using Physics2D.Object.Tools;

namespace Physics2D.Object
{
    /// <summary>
    /// 由多个质点组合成的物体
    /// 质点由刚性连杆连接，可获得类似刚体的自由度
    /// </summary>
    public class CombinedParticle : CustomObject, IPin
    {
        #region 私有字段
        /// <summary>
        /// 质体顶点表（顺时针）
        /// </summary>
        private readonly List<Particle> _vertexs = new List<Particle>();

        /// <summary>
        /// 刚性连杆表（顺时针）
        /// </summary>
        private readonly List<ParticleRod> _rods = new List<ParticleRod>();

        /// <summary>
        /// 锚点连杆表
        /// </summary>
        private List<ParticleRod> _pinRods = new List<ParticleRod>();

        /// <summary>
        /// 是否封闭（首尾相连）
        /// </summary>
        private readonly bool _isClose;
        #endregion

        #region 公开的属性
        /// <summary>
        /// 质体顶点表（顺时针）
        /// </summary>
        public IReadOnlyList<Particle> Vertexs => _vertexs;

        /// <summary>
        /// 刚性连杆表（顺时针）
        /// </summary>
        public IReadOnlyList<ParticleRod> Rods => _rods;

        /// <summary>
        /// 是否封闭（首尾相连）
        /// </summary>
        public bool IsClose => _isClose;

        public IReadOnlyList<ParticleRod> PinRods => _pinRods;
        #endregion

        #region 构造方法
        /// <summary>
        /// 创建一个联合质体
        /// </summary>
        /// <param name="vertexs"></param>
        /// <param name="velocity"></param>
        /// <param name="mass"></param>
        /// <param name="restitution"></param>
        /// <param name="isClose"></param>
        public CombinedParticle(IEnumerable<Vector2D> vertexs, Vector2D velocity, double mass, double restitution, bool isClose)
        {
            var num = vertexs.Count();
            if(num < 2)
            {
                // TODO: 需要抛出异常
            }
            foreach(var vertex in vertexs)
            {
                _vertexs.Add(new Particle
                {
                    Position = vertex,
                    Velocity = velocity,
                    Mass = mass / num,
                    Restitution = restitution
                });
            }

            // 在可形成多边形的时候允许链接为封闭图形
            _isClose = isClose;
            if(isClose && num > 2)
            {
                _vertexs.Add(_vertexs[0]);
            }
            // 创建刚性连杆
            for(int i = 1; i < _vertexs.Count; i++)
            {
                _rods.Add(new ParticleRod(_vertexs[i - 1], _vertexs[i]));
            }
        }
        #endregion

        #region 自定义物体接口实现
        /// <summary>
        /// 装载到物理世界
        /// </summary>
        /// <param name="world"></param>
        public override void OnInit(World world)
        {
            foreach(var vertex in _vertexs)
            {
                world.AddObject(vertex);
            }
            foreach(var rod in _rods)
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
            foreach (var vertex in _vertexs)
            {
                world.RemoveObject(vertex);
            }
        }
        #endregion

        #region 物体抽象方法实现
        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="duration"></param>
        public override void Update(double duration)
        {
            // 组织质体的位移被视为所有质体的偏移量
            // 该偏移量在每次施加偏移过后置0
            var N = _isClose ? _vertexs.Count - 1 : _vertexs.Count;
            for (int i = 0; i < N; i++)
            {
                _vertexs[i].Position += Position;
            }
            Position = Vector2D.Zero;
        }
        #endregion

        #region 固定物体接口实现
        /// <summary>
        /// 让该物体与指定Pin点连接
        /// </summary>
        /// <param name="world"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Particle Pin(World world, Vector2D position)
        {
            var pin = new Particle
            {
                Position = position,
                InverseMass = 0
            };

            // 对于封闭图形，任意连接两个点即可固定住形状
            // 对于不封闭图形，需要每个点都连接才可固定
            var N = _isClose ? 2 : _vertexs.Count;
            for(int i = 0; i < N; i++)
            {
                _pinRods.Add(world.CreateRod(_vertexs[i], pin));
            }
            
            return pin;
        }

        /// <summary>
        /// 取消该物体与所有Pin的连接
        /// </summary>
        /// <param name="world"></param>
        public void UnPin(World world)
        {
            // 移除连接
            foreach(var rod in _pinRods)
            {
                world.ContactGenerators.Remove(rod);
            }
            _pinRods.Clear();
            // 恢复速度
            foreach (var vertex in _vertexs)
            {
                vertex.Velocity = Velocity;
            }
        }

        Handle IPin.Pin(World world, Vector2D position)
        {
            throw new NotImplementedException();
        }

        void IPin.UnPin(World world)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
