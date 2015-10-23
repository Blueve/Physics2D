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

namespace Physics2D.Object
{
    /// <summary>
    /// 由多个质点组合成的物体
    /// 质点由刚性连杆连接，可获得类似刚体的自由度
    /// </summary>
    public class CombinedParticle : CustomObject
    {
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

        private bool _isClose;

        /// <summary>
        /// 质体顶点表（顺时针）
        /// </summary>
        public IReadOnlyList<Particle> Vertexs
        {
            get { return _vertexs; }
        }

        /// <summary>
        /// 刚性连杆表（顺时针）
        /// </summary>
        public IReadOnlyList<ParticleRod> Rods
        {
            get { return _rods; }
        }

        public IReadOnlyList<ParticleRod> PinRods
        {
            get { return _pinRods; }
        }

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
            foreach (var rod in _rods)
            {
                world.ContactGenerators.Remove(rod);
            }
        }

        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="duration"></param>
        public override void Update(double duration)
        {
            var N = _isClose ? _vertexs.Count - 1 : _vertexs.Count;
            for (int i = 0; i < N; i++)
            {
                _vertexs[i].Position += Position;
            }
            Position = Vector2D.Zero;
        }

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

            var N = _isClose ? _vertexs.Count - 1 : _vertexs.Count;
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
            foreach (var pin in _pinRods)
            {
                world.ContactGenerators.Remove(pin);
            }
            _pinRods.Clear();
        }
    }
}
