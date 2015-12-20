using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Physics2D;
using Physics2D.Collision;
using Physics2D.Collision.Basic;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Force.Zones;
using Physics2D.Object;
using WPFDemo.Graphic;
using Physics2D.Collision.Shapes;

namespace WPFDemo.FireworksDemo
{

    public class FireworksDemo : PhysicsGraphic, IDrawable
    {
        #region 公开的内部类型
        /// <summary>
        /// 选项状态枚举
        /// </summary>
        public enum PhysicsType
        {
            None,
            Water,
            Wind
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 当前选项
        /// </summary>
        private PhysicsType _type = PhysicsType.None;
        #endregion

        #region 公开的属性
        /// <summary>
        /// 当前选项
        /// </summary>
        public PhysicsType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                // 设置力场
                if (_type == PhysicsType.Water)
                {
                    PhysicsWorld.Zones.Add(_dragZone);
                    PhysicsWorld.Zones.Remove(_windZone);
                }
                else if (_type == PhysicsType.Wind)
                {
                    PhysicsWorld.Zones.Add(_windZone);
                    PhysicsWorld.Zones.Remove(_dragZone);
                }
            }
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 阻力
        /// </summary>
        private readonly ParticleDrag _drag = new ParticleDrag(2, 1);
        /// <summary>
        /// 风
        /// </summary>
        private readonly ParticleConstantForce _wind = new ParticleConstantForce(new Vector2D(20, -5));
        /// <summary>
        /// 阻力区
        /// </summary>
        private readonly Zone _dragZone;
        /// <summary>
        /// 有风区
        /// </summary>
        private readonly Zone _windZone;
        /// <summary>
        /// 横板
        /// </summary>
        private Edge _edge;
        /// <summary>
        /// 物体列表
        /// </summary>
        private readonly List<Particle> _objList = new List<Particle>();
        /// <summary>
        /// 粒子形状Id
        /// 所有粒子都使用相同的Id，从而使粒子之间可以交叠
        /// </summary>
        private readonly int _shapeId = Shape.NewId();
        /// <summary>
        /// 粒子半径
        /// </summary>
        private const int BallSize = 4;
        #endregion

        #region 私有的常量字段
        private const int WorldHeight = 400;
        private const int WorldWidth = 500;
        #endregion

        #region 构造方法
        public FireworksDemo(Image image)
            : base(image)
        {
            DrawQueue.Add(this);

            _dragZone = new RectangleZone
            (
                0.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                400.ToSimUnits()
            );
            _dragZone.Add(_drag);

            _windZone = new RectangleZone
            (
                0.ToSimUnits(),
                (WorldHeight * 1 / 3.0).ToSimUnits(),
                500.ToSimUnits(),
                (WorldHeight * 2 / 3.0).ToSimUnits()
            );
            _windZone.Add(_wind);
        }
        #endregion

        #region 实现PhysicsGraphic
        protected override void UpdatePhysics(double duration)
        {
            PhysicsWorld.Update(duration);
        }
        #endregion

        #region 实现IDrawable
        public void Draw(WriteableBitmap bitmap)
        {
            
            if (_type == PhysicsType.Water)
                bitmap.FillRectangle(0, WorldHeight * 2 / 3, WorldWidth, WorldHeight, Colors.SkyBlue);
            else if (_type == PhysicsType.Wind)
                bitmap.FillRectangle(0, WorldHeight * 1 / 3, WorldWidth, WorldHeight * 2 / 3, Colors.LightGray);

            bitmap.DrawLineAa(
                _edge.PointA.X.ToDisplayUnits(),
                _edge.PointA.Y.ToDisplayUnits(),
                _edge.PointB.X.ToDisplayUnits(),
                _edge.PointB.Y.ToDisplayUnits(), Colors.Black);

            for (int i = _objList.Count - 1; i >= 0; i--)
            {
                int x = _objList[i].Position.X.ToDisplayUnits();
                int y = _objList[i].Position.Y.ToDisplayUnits();

                if (y > WorldHeight || x > WorldWidth || x < 0 || y < 0)
                {
                    PhysicsWorld.RemoveObject(_objList[i]);
                    _objList.Remove(_objList[i]);
                }
                else
                {
                    if(_type == PhysicsType.Water)
                    {
                        bitmap.FillEllipseCentered(x, y, BallSize, BallSize, y > WorldHeight * 2 / 3 ? Colors.DarkBlue : Colors.Black);
                    }
                    else
                    {
                        bitmap.FillEllipseCentered(x, y, BallSize, BallSize, Colors.Black);
                    }
                }
            }
        }
        #endregion

        #region 响应鼠标事件
        public void Fire(double x, double y)
        {
            if (!Start)
            {
                Start = true;
                // 增加重力
                PhysicsWorld.CreateGravity(9.8);
                // 添加边缘
                _edge = PhysicsWorld.CreateEdge(
                    100.ToSimUnits(),
                    350.ToSimUnits(),
                    400.ToSimUnits(),
                    200.ToSimUnits());
                Slot = 1 / 60.0;
            }

            var rnd = new Random();
            
            for (int i = 0; i < 10; i++)
            {
                var paritcle = PhysicsWorld.CreateParticle
                (
                    new Vector2D(x, y),
                    new Vector2D(rnd.NextDouble() * 6 - 3, rnd.NextDouble() * 6 - 3),
                    1f, 0.1
                );
                paritcle.BindShape(new Circle(BallSize.ToSimUnits(), _shapeId));
                _objList.Add(paritcle);
            }
        }
        #endregion

    }
}