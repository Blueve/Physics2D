using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Collision.Shapes;
using Physics2D.Common;
using Physics2D.Core;

namespace Physics2D.Object
{
    /// <summary>
    /// 流体
    /// </summary>
    public class Fluid : CustomObject
    {
        private World _world;

        private readonly List<FluidParticle> _particles = new List<FluidParticle>();

        private double _smoothR;
        private double _viscosity;
        private double _pressure;

        private double _kPoly6;
        private double _kSpiky;
        private double _kViscosity;

        private double _restDensity;
        private double _gasConstantK;

        private double _particleMass;

        private double _speedLimiting;

        public double ParticleDistance => Math.Pow(_particleMass / _restDensity, 1.0 / 3);

        public IReadOnlyList<Particle> Particles => _particles; 

        public Fluid()
        {
            _smoothR = 125.ToSimUnits();
            //_restDensity = .5;
            //_pressure = 2;
            //_viscosity = 0.075;
            //_particleMass = 1;

            //_smoothR = 0.01;
            _restDensity = 0.02;
            _gasConstantK = 1;
            _particleMass = 0.0008;
            _viscosity = 1;

            _speedLimiting = 200;

            _kPoly6 = 4 / (Math.PI * Math.Pow(_smoothR, 8));
            _kSpiky = -30 / (Math.PI * Math.Pow(_smoothR, 5));
            _kViscosity = 20 / (3 * Math.PI * Math.Pow(_smoothR, 5));

            //_kPoly6 = 315 / (64 * Math.PI * Math.Pow(_smoothR, 9));
            //_kSpiky = -45 / (Math.PI * Math.Pow(_smoothR, 6));
            //_kViscosity = 45 / (3 * Math.PI * Math.Pow(_smoothR, 6));

            System.Diagnostics.Debug.WriteLine("Space\t" + ParticleDistance.ToDisplayUnits());

            System.Diagnostics.Debug.WriteLine("kPoly6\t" + _kPoly6);
            System.Diagnostics.Debug.WriteLine("kSpiky\t" + _kSpiky);
            System.Diagnostics.Debug.WriteLine("kViscosity\t" + _kViscosity);
        }

        public void Add(Particle particle)
        {
            var p = new FluidParticle
            {
                Position = particle.Position,
                Velocity = particle.Velocity,
                Mass = _particleMass,
                Restitution = 1
            };
            
            p.BindShape(new Circle(4.ToSimUnits(), 1));
            _particles.Add(p);

            if(_world != null)
            {
                _world.AddObject(p);
                //_world.AddShape(p.Shape);
            }
        }

        public override void Update(double duration)
        {
            var h2 = _smoothR * _smoothR;

            // 计算密度
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Neighbors.Clear();
                _particles[i].Density = 0;
                for (int j = 0; j < i; j++)
                {
                    var d2 = Vector2D.DistanceSquared(_particles[i].Position, _particles[j].Position);
                    if(h2 > d2)
                    {
                        var weight = Math.Pow(h2 - d2, 3);
                        //var weight = Math.Pow(1 - Math.Sqrt(d2) / _smoothR, 2);
                        //if (_particles[i].Neighbors.Count < 7)
                            _particles[i].Density += weight;
                        //if (_particles[j].Neighbors.Count < 7)
                            _particles[j].Density += weight;

                        if (_particles[i].Neighbors.Count < 80)
                            _particles[i].Neighbors.Add(_particles[j]);
                        if(_particles[j].Neighbors.Count < 80)
                            _particles[j].Neighbors.Add(_particles[i]);
                        
                    }
                }
            }
            // 计算压力
            foreach (var particle in _particles)
            {
                particle.Density += Math.Pow(h2, 3);
                particle.Density *= _kPoly6 * _particleMass;

                //if (particle.Density < _restDensity) particle.Density = _restDensity;
                System.Diagnostics.Debug.WriteLine(particle.Density);
                particle.Pressure = (particle.Density - _restDensity) * _gasConstantK;
            }
            // 计算合力
            for (int i = 0; i < _particles.Count; i++)
            {
                var force = Vector2D.Zero;
                for (int j = 0; j < _particles[i].Neighbors.Count; j++)
                {
                    var neighbor = _particles[i].Neighbors[j];

                    //var d = Vector2D.Distance(_particles[i].Position, neighbor.Position);
                    //var weight = 1 - d / _smoothR;
                    //var pressure = 80 * weight * (_particles[i].Pressure + neighbor.Pressure) / (2 * _particles[i].Density * neighbor.Density) * _pressure;
                    //var pij = _particles[i].Position - neighbor.Position;
                    //_particles[i].AddForce(pij / (d + 0.0000001) * pressure);

                    //var viscosity = 70 * weight / neighbor.Density * _viscosity;
                    //var vij = _particles[i].Velocity - neighbor.Velocity;
                    //_particles[i].AddForce(-vij * viscosity);

                    var r = Vector2D.Distance(_particles[i].Position, neighbor.Position);
                    var pij = _particles[i].Position - neighbor.Position;
                    var hr = _smoothR - r;
                    var h2r2 = h2 - r * r;

                    var pterm = -_particleMass * _kSpiky * hr * hr * (_particles[i].Pressure + neighbor.Pressure) / (2 * _particles[i].Density * neighbor.Density);
                    force += pij * pterm / r;

                    var vterm = _particleMass * _kViscosity * _viscosity * hr / (_particles[i].Density * neighbor.Density);
                    force += (neighbor.Velocity - _particles[i].Velocity) * vterm;
                }
                _particles[i].AddForce(force * _particleMass);
            }

            //foreach (var particle in _particles)
            //{
                //particle.AddForce(new Vector2D(0, 9.8) * _particleMass);
                //particle.Update(duration);
            //}
            //    System.Diagnostics.Debug.WriteLine(
            //    _particles[0].Density.ToString() + "\t" +
            //    _particles[1].Density.ToString());
            //    System.Diagnostics.Debug.WriteLine(
            //    _particles[0].Pressure.ToString() + "\t" +
            //    _particles[1].Pressure.ToString());
            //    System.Diagnostics.Debug.WriteLine(
            //    _particles[0].Position.ToDisplayUnits().ToString() + 
            //    _particles[1].Position.ToDisplayUnits().ToString());
            //System.Diagnostics.Debug.WriteLine("-----------------------------");
        }

        public override void OnInit(World world)
        {
            _world = world;
            foreach (var particle in _particles)
            {
                _world.AddObject(particle);
            }
        }

        public override void OnRemove(World world)
        {
            throw new NotImplementedException();
        }

        class FluidParticle : Particle
        {
            public readonly List<FluidParticle> Neighbors = new List<FluidParticle>();
            public double Density = 0;
            public double Pressure = 0;
        }
    }
}
