using Physics2D;
using Physics2D.Collision;
using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Physics2D.Collision.Basic;
using WPFDemo.Graphic;
using Physics2D.Collision.Shapes;

namespace WPFDemo.FluidDemo
{
    public class FluidDemo : PhysicsGraphic, IDrawable
    {
        private readonly Physics2D.Object.Fluid _fluid;
        private int _pNum = 20;

        private readonly Water _water;
        private readonly Edge _edge0, _edge1, _edge2, _edge3;
        private readonly Vector2D _center = new Vector2D(ConvertUnits.ToSimUnits(250f), ConvertUnits.ToSimUnits(200f));

        public FluidDemo(Image image)
            : base(image)
        {
            Slot = 1 / 120.0;
            _fluid = new Physics2D.Object.Fluid();

            _edge0 = PhysicsWorld.CreateEdge(
                50.ToSimUnits(),
                50.ToSimUnits(),
                50.ToSimUnits(),
                350.ToSimUnits());
            _edge1 = PhysicsWorld.CreateEdge(
                450.ToSimUnits(),
                50.ToSimUnits(),
                450.ToSimUnits(),
                350.ToSimUnits());
            _edge2 = PhysicsWorld.CreateEdge(
                50.ToSimUnits(),
                350.ToSimUnits(),
                450.ToSimUnits(),
                350.ToSimUnits());
            _edge3 = PhysicsWorld.CreateEdge(
                50.ToSimUnits(),
                50.ToSimUnits(),
                450.ToSimUnits(),
                50.ToSimUnits());
            // 创建液体容器
            _water = new Water((int)image.Width, (int)image.Height);
            // 添加到绘制队列
            //DrawQueue.Add(_water);

            //var particleDistance = 18;
            var particleDistance = _fluid.ParticleDistance.ToDisplayUnits();
            for(int i = 150; i < 350; i += particleDistance)
            {
                for(int j = 150; j < 340; j += particleDistance)
                {
                    var p = new Particle
                    {
                        Position = new Vector2D(i.ToSimUnits(), j.ToSimUnits())
                    };
                    _fluid.Add(p);
                }
            }
            foreach(var p in _fluid.Particles)
            {
                _water.ObjList.Add(p);
            }


            //while (_pNum-- > 0)
            //{
            //    var p = new Particle
            //    {
            //        Position = new Vector2D(0, 20.ToSimUnits()),
            //        Velocity = new Vector2D(50.ToSimUnits(), 0)
            //    };
            //    _fluid.Add(p);
            //}
            //PhysicsWorld.AddObject(_fluid.Particles[_fluid.Particles.Count - 1]);

            PhysicsWorld.AddCustomObject(_fluid);
            DrawQueue.Add(this);
        }

        protected override void UpdatePhysics(double duration)
        {
            //if(!_flag)
            //    foreach (var item in _water.ObjList)
            //    {
            //        Vector2D v = _center - item.Position;
            //        double d = v.Length();
            //        item.AddForce(v.Normalize() * 5 * d);
            //    }
            //if (_pNum-- > 0)
            //{
            //    var p = new Particle
            //    {
            //        Position = new Vector2D(100.ToSimUnits(), 20.ToSimUnits()),
            //        Velocity = new Vector2D(10.ToSimUnits(), 0)
            //    };
            //    _fluid.Add(p);
            //}
            //PhysicsWorld.AddObject(_fluid.Particles[_fluid.Particles.Count - 1]);
            PhysicsWorld.Update(duration);
        }

        public void Fire()
        {
            Random rnd = new Random();
            if (!Start)
            {
                Start = true;
                PhysicsWorld.CreateGravity(9.8);
                //// 设置全局的阻力
                //PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.5, 0.5));
                
                //// 初始化水
                //for (int i = 0; i < 20; i++)
                //{
                //    var item = PhysicsWorld.CreateParticle
                //    (
                //        new Vector2D
                //        (
                //            rnd.Next((int)Bitmap.Width).ToSimUnits(),
                //            rnd.Next((int)Bitmap.Height).ToSimUnits()
                //        ),
                //        Vector2D.Zero,
                //        1
                //    );
                //    item.BindShape(new Circle(2.ToSimUnits()));
                //    _water.ObjList.Add(item);
                //}
            }
            // 抖动
            //_water.ObjList.ForEach(obj => obj.Velocity.Set(rnd.Next(5) - 2.5, rnd.Next(5) - 2.5));
        }

        private bool _flag = false;

        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.DrawLineAa(
                _edge0.PointA.X.ToDisplayUnits(),
                _edge0.PointA.Y.ToDisplayUnits(),
                _edge0.PointB.X.ToDisplayUnits(),
                _edge0.PointB.Y.ToDisplayUnits(), Colors.Black);
            bitmap.DrawLineAa(
                _edge1.PointA.X.ToDisplayUnits(),
                _edge1.PointA.Y.ToDisplayUnits(),
                _edge1.PointB.X.ToDisplayUnits(),
                _edge1.PointB.Y.ToDisplayUnits(), Colors.Black);
            bitmap.DrawLineAa(
                _edge2.PointA.X.ToDisplayUnits(),
                _edge2.PointA.Y.ToDisplayUnits(),
                _edge2.PointB.X.ToDisplayUnits(),
                _edge2.PointB.Y.ToDisplayUnits(), Colors.Black);

            bitmap.DrawLineAa(
                _edge3.PointA.X.ToDisplayUnits(),
                _edge3.PointA.Y.ToDisplayUnits(),
                _edge3.PointB.X.ToDisplayUnits(),
                _edge3.PointB.Y.ToDisplayUnits(), Colors.Black);

            foreach (var particle in _fluid.Particles)
            {
                bitmap.FillEllipseCentered(particle.Position.X.ToDisplayUnits(), particle.Position.Y.ToDisplayUnits(), 1, 1, Colors.DarkBlue);
            }
        }
    }
}