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
        private World world;

        private Particle[,] net;
        private int width;
        private int height;

        private Vector2D startPosition;
        private float gridSize;

        public ElasticatedNet(World world, Vector2D startPosition, int width, int height, float gridSize)
        {
            this.world         = world;
            this.width         = width;
            this.height        = height;
            this.startPosition = startPosition;
            this.gridSize      = gridSize;

            net = new Particle[width, height];
        }

        public void Reset()
        {
            // 设置全局的阻力
            ZoneFactory.CreateGlobalZone(world, new ParticleDrag(0.8f, 0.6f));
            // 清除以往的的数据
            foreach (var item in net)
            {
                if (item != null) world.RemoveObject(item);
            }
            // 根据参数创建弹性网
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    net[i, j] = world.CreateParticle
                    (
                        new Vector2D(
                            (startPosition.X + i * gridSize),
                            (startPosition.Y + j * gridSize)
                        ),
                        Vector2D.Zero,
                        1f
                    );
                }
            }
            // 设置弹性网
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var spring = new DestructibleElastic(12, gridSize);
                    if (i > 0         ) spring.Add(net[i - 1, j]);
                    if (i < width - 1 ) spring.Add(net[i + 1, j]);
                    if (j > 0         ) spring.Add(net[i, j - 1]);
                    if (j < height - 1) spring.Add(net[i, j + 1]);
                    world.RegistryForceGenerator(net[i, j], spring);
                }
            }
            // 对弹性网进行拉扯
            for (int i = 0; i < width; i++)
            {
                net[i, height - 1].Velocity    = new Vector2D(0f, 0.1f) * 4 * (i >= width / 2 ? -1 : 1);
                net[i, height - 1].InverseMass = 0f;
            }
        }

        /// <summary>
        /// 实现图形接口的绘图逻辑
        /// </summary>
        /// <param name="bitmap"></param>
        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.Clear();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int x = ConvertUnits.ToDisplayUnits(net[i, j].Position.X);
                    int y = ConvertUnits.ToDisplayUnits(net[i, j].Position.Y);

                    // 绘制弹性连线
                    float dLeft;
                    float dDown;
                    if (i > 0 && (dLeft = (net[i, j].Position - net[i - 1, j].Position).Length()) < 1.5f)
                    {
                        byte colorRow = dLeft > gridSize ? (byte)((int)(255 - (dLeft - gridSize) * 150)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            ConvertUnits.ToDisplayUnits(net[i - 1, j].Position.X), ConvertUnits.ToDisplayUnits(net[i - 1, j].Position.Y),
                            Color.FromArgb(colorRow, 0, 0, 0));
                    }
                    if (j > 0 && (dDown = (net[i, j].Position - net[i, j - 1].Position).Length()) < 1.5f)
                    {
                        byte colorCol = dDown > gridSize ? (byte)((int)(255 - (dDown - gridSize) * 150)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            ConvertUnits.ToDisplayUnits(net[i, j - 1].Position.X), ConvertUnits.ToDisplayUnits(net[i, j - 1].Position.Y),
                            Color.FromArgb(colorCol, 0, 0, 0));
                    }
                    // 绘制点
                    bitmap.FillEllipseCentered(x, y, 2, 2, Colors.Black);
                }
            }
        }
    }
}