using Physics2D;
using Physics2D.Common;
using Physics2D.Core;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D.Object;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFDemo.Graphic;
using System;

namespace WPFDemo.ElasticDemo
{
    internal sealed class ElasticatedNet : CustomObject, IDrawable
    {
        #region 私有字段
        private readonly Particle[,] _net;
        private readonly int _width;
        private readonly int _height;
        private readonly Vector2D _startPosition;
        private readonly double _gridSize;
        private readonly double _factor;
        #endregion

        #region 公开属性
        /// <summary>
        /// 弹簧网网格
        /// </summary>
        public Particle[,] Net
        {
            get { return _net; }
        }
        #endregion

        #region 构造方法
        public ElasticatedNet(Vector2D startPosition, int width, int height, double gridSize, double factor)
        {
            _width         = width;
            _height        = height;
            _startPosition = startPosition;
            _gridSize      = gridSize;
            _factor        = factor;
            _net = new Particle[width, height];
        }
        #endregion

        #region 实现IDrawable
        /// <summary>
        /// 实现图形接口的绘图逻辑
        /// </summary>
        /// <param name="bitmap"></param>
        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.Clear();
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    int x = _net[i, j].Position.X.ToDisplayUnits();
                    int y = _net[i, j].Position.Y.ToDisplayUnits();

                    // 绘制弹性连线
                    double dLeft;
                    double dDown;
                    if (i > 0 && (dLeft =(_net[i, j].Position - _net[i - 1, j].Position).Length()) / _gridSize < _factor)
                    {
                        byte colorRow = dLeft > _gridSize ? (byte)((int)(255 - (dLeft - _gridSize) / ((_factor - 1) * _gridSize) * 200)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            _net[i - 1, j].Position.X.ToDisplayUnits(), _net[i - 1, j].Position.Y.ToDisplayUnits(),
                            Color.FromArgb(colorRow, 0, 0, 0));
                    }
                    if (j > 0 && (dDown = (_net[i, j].Position - _net[i, j - 1].Position).Length()) / _gridSize < _factor)
                    {
                        byte colorCol = dDown > _gridSize ? (byte)((int)(255 - (dDown - _gridSize) / ((_factor - 1) * _gridSize) * 200)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            _net[i, j - 1].Position.X.ToDisplayUnits(), _net[i, j - 1].Position.Y.ToDisplayUnits(),
                            Color.FromArgb(colorCol, 0, 0, 0));
                    }
                    // 绘制点
                    bitmap.FillEllipseCentered(x, y, 2, 2, Colors.Black);
                }
            }
        }
        #endregion

        #region 实现CustomObject
        public override void OnInit(World world)
        {
            // 根据参数创建弹性网
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _net[i, j] = world.CreateParticle
                    (
                        new Vector2D(
                            (_startPosition.X + i * _gridSize),
                            (_startPosition.Y + j * _gridSize)
                        ),
                        Vector2D.Zero,
                        1
                    );
                }
            }
            // 设置弹性网
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    var spring = new DestructibleElastic(12, _gridSize, _factor);
                    if (i > 0) spring.Joint(_net[i - 1, j]);
                    if (i < _width - 1) spring.Joint(_net[i + 1, j]);
                    if (j > 0) spring.Joint(_net[i, j - 1]);
                    if (j < _height - 1) spring.Joint(_net[i, j + 1]);
                    spring.Add(_net[i, j]);
                    world.ForceGenerators.Add(spring);
                }
            }
        }

        public override void OnRemove(World world)
        {
            foreach (var item in _net)
            {
                world.RemoveObject(item);
            }
        }

        public override void Update(double duration)
        {
        }
        #endregion
    }
}