using Physics2D;
using Physics2D.Common;
using Physics2D.Core;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D.Object;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFDemo.Graphic;

namespace WPFDemo.ElasticDemo
{
    internal class ElasticatedNet : IDrawable
    {
        private readonly World _world;

        private readonly Particle[,] _net;
        private readonly int _width;
        private readonly int _height;

        private Vector2D _startPosition;
        private readonly double _gridSize;

        public ElasticatedNet(World world, Vector2D startPosition, int width, int height, double gridSize)
        {
            this._world         = world;
            this._width         = width;
            this._height        = height;
            this._startPosition = startPosition;
            this._gridSize      = gridSize;

            _net = new Particle[width, height];
        }

        public void Reset()
        {
            // 清除以往的的数据
            foreach (var item in _net)
            {
                if (item != null) _world.RemoveObject(item);
            }
            // 根据参数创建弹性网
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _net[i, j] = _world.CreateParticle
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
                    var spring = new DestructibleElastic(12, _gridSize);
                    if (i > 0          ) spring.Add(_net[i - 1, j]);
                    if (i < _width - 1 ) spring.Add(_net[i + 1, j]);
                    if (j > 0          ) spring.Add(_net[i, j - 1]);
                    if (j < _height - 1) spring.Add(_net[i, j + 1]);
                    _world.RegistryForceGenerator(_net[i, j], spring);
                }
            }
            // 对弹性网进行拉扯
            for (int i = 0; i < _width; i++)
            {
                _net[i, _height - 1].Velocity    = new Vector2D(0, 0.1) * 4 * (i >= _width / 2 ? -1 : 1);
                _net[i, _height - 1].InverseMass = 0;
            }
        }

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
                    if (i > 0 && (dLeft = (_net[i, j].Position - _net[i - 1, j].Position).Length()) < 1.5)
                    {
                        byte colorRow = dLeft > _gridSize ? (byte)((int)(255 - (dLeft - _gridSize) * 150)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            _net[i - 1, j].Position.X.ToDisplayUnits(), _net[i - 1, j].Position.Y.ToDisplayUnits(),
                            Color.FromArgb(colorRow, 0, 0, 0));
                    }
                    if (j > 0 && (dDown = (_net[i, j].Position - _net[i, j - 1].Position).Length()) < 1.5)
                    {
                        byte colorCol = dDown > _gridSize ? (byte)((int)(255 - (dDown - _gridSize) * 150)) : (byte)255;
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
    }
}