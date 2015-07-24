using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Physics2D;
using Physics2D.Object;

using WPFDemo.Graphic;

namespace WPFDemo.FluidDemo
{
    class Water : IDrawable
    {
        public readonly List<Particle> objList = new List<Particle>();

        private const int threshold = 900;
        private const int gridR = 60;

        private const int R = 150;

        private int[] metaTable;
        private int[,] cacheTable;
        private object[,] cacheLocks;
        private int[,] cache;

        public Water(int maxWidth, int maxHeight)
        {
            // 计算势能函数缓存
            metaTable = new int[gridR];
            metaTable[0] = threshold;
            for (int i = 1; i < gridR; i++)
            {
                metaTable[i] = (R * R / (i * i));
            }
            // 计算势能缓存
            cacheTable = new int[2 * gridR, 2 * gridR];
            for (int i = 0; i < 2 * gridR; i++)
            {
                for (int j = 0; j < 2 * gridR; j++)
                {
                    int d = (int)Math.Sqrt((i - gridR) * (i - gridR) + (j - gridR) * (j - gridR) + 0.5);
                    cacheTable[i, j] = d < gridR ? metaTable[d] : 0;
                }
            }
            // 初始化锁
            int w = maxWidth;
            int h = maxHeight;
            cacheLocks = new object[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    cacheLocks[i, j] = new object();
                }
            }
        }

        public unsafe void Draw(WriteableBitmap bitmap)
        {
            // 绘制Metaball
            using (var wc = bitmap.GetBitmapContext())
            {
                int w = wc.Width;
                int h = wc.Height;
                var pixels = wc.Pixels;
                cache = cache ?? new int[w, h];
                Array.Clear(cache, 0, cache.Length);
                // 叠加每个球的势能
                Parallel.ForEach(objList, obj =>
                {
                    int x = ConvertUnits.ToDisplayUnits(obj.Position.X);
                    int y = ConvertUnits.ToDisplayUnits(obj.Position.Y);

                    for (int i = x - gridR, I = 0; i < x + gridR; i++, I++)
                    {
                        for (int j = y - gridR, J = 0; j < y + gridR; j++, J++)
                        {
                            if (i < 0 || i >= w || j < 0 || j >= h) continue;
                            else
                            {
                                lock (cacheLocks[i, j])
                                {
                                    cache[i, j] += cacheTable[I, J];
                                }
                            }
                        }
                    }
                });
                // 渲染画布
                Parallel.For(0, h, y =>
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (cache[x, y] >= threshold)
                        {
                            pixels[y * w + x] = (255 << 24) | (16 << 68) | (146 << 8) | (216);
                        }
                    }
                });
            }
        }
    }
}
