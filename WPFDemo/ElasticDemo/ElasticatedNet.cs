namespace WPFDemo.ElasticDemo
{
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Physics2D;
    using Physics2D.Common;
    using Physics2D.Core;
    using Physics2D.Factories;
    using Physics2D.Object;
    using WPFDemo.Graphic;

    internal sealed class ElasticatedNet : CustomObject, IDrawable
    {
        private readonly Particle[,] net;
        private readonly int width;
        private readonly int height;
        private readonly Vector2D startPosition;
        private readonly double gridSize;
        private readonly double factor;

        /// <summary>
        /// 弹簧网网格
        /// </summary>
        public Particle[,] Net
        {
            get { return this.net; }
        }

        public ElasticatedNet(Vector2D startPosition, int width, int height, double gridSize, double factor)
        {
            this.width = width;
            this.height = height;
            this.startPosition = startPosition;
            this.gridSize = gridSize;
            this.factor = factor;
            this.net = new Particle[width, height];
        }

        /// <summary>
        /// 实现图形接口的绘图逻辑
        /// </summary>
        /// <param name="bitmap"></param>
        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.Clear();
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    int x = this.net[i, j].Position.X.ToDisplayUnits();
                    int y = this.net[i, j].Position.Y.ToDisplayUnits();

                    // 绘制弹性连线
                    double dLeft;
                    double dDown;
                    if (i > 0 && (dLeft = (this.net[i, j].Position - this.net[i - 1, j].Position).Length()) / this.gridSize < this.factor)
                    {
                        byte colorRow = dLeft > this.gridSize ? (byte)((int)(255 - (dLeft - this.gridSize) / ((this.factor - 1) * this.gridSize) * 200)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            this.net[i - 1, j].Position.X.ToDisplayUnits(), this.net[i - 1, j].Position.Y.ToDisplayUnits(),
                            Color.FromArgb(colorRow, 0, 0, 0));
                    }

                    if (j > 0 && (dDown = (this.net[i, j].Position - this.net[i, j - 1].Position).Length()) / this.gridSize < this.factor)
                    {
                        byte colorCol = dDown > this.gridSize ? (byte)((int)(255 - (dDown - this.gridSize) / ((this.factor - 1) * this.gridSize) * 200)) : (byte)255;
                        bitmap.DrawLineAa(
                            x, y,
                            this.net[i, j - 1].Position.X.ToDisplayUnits(), this.net[i, j - 1].Position.Y.ToDisplayUnits(),
                            Color.FromArgb(colorCol, 0, 0, 0));
                    }

                    // 绘制点
                    bitmap.FillEllipseCentered(x, y, 2, 2, Colors.Black);
                }
            }
        }

        public override void OnInit(World world)
        {
            // 根据参数创建弹性网
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    this.net[i, j] = world.CreateParticle(
                        new Vector2D(
                            this.startPosition.X + i * this.gridSize,
                            this.startPosition.Y + j * this.gridSize),
                        Vector2D.Zero,
                        1);
                }
            }

            // 设置弹性网
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    var spring = new DestructibleElastic(12, this.gridSize, this.factor);
                    if (i > 0)
                    {
                        spring.Joint(this.net[i - 1, j]);
                    }

                    if (i < this.width - 1)
                    {
                        spring.Joint(this.net[i + 1, j]);
                    }

                    if (j > 0)
                    {
                        spring.Joint(this.net[i, j - 1]);
                    }

                    if (j < this.height - 1)
                    {
                        spring.Joint(this.net[i, j + 1]);
                    }

                    spring.Add(this.net[i, j]);
                    world.ForceGenerators.Add(spring);
                }
            }
        }

        public override void OnRemove(World world)
        {
            foreach (var item in this.net)
            {
                world.RemoveObject(item);
            }
        }

        public override void Update(double duration)
        {
        }
    }
}