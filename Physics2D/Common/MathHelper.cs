namespace Physics2D.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static class MathHelper
    {
        /// <summary>
        /// 计算一个点到一个线段的向量
        /// 返回点到线的距离最短的向量
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="linePA">线段端点A</param>
        /// <param name="linePB">线段端点B</param>
        /// <returns></returns>
        public static Vector2D PointToLineVector(Vector2D point, Vector2D linePA, Vector2D linePB)
        {
            var lineBA = linePB - linePA;
            var normal = lineBA * (point - linePA) * lineBA / lineBA.LengthSquared() - (point - linePA);

            return normal;
        }

        /// <summary>
        /// 计算两个线段的交点
        /// </summary>
        /// <param name="lineA1"></param>
        /// <param name="lineA2"></param>
        /// <param name="lineB1"></param>
        /// <param name="lineB2"></param>
        /// <returns></returns>
        public static Vector2D? LineIntersection(Vector2D lineA1, Vector2D lineA2, Vector2D lineB1, Vector2D lineB2)
        {
            var area1 = SignedTriangleArea(lineA1, lineA2, lineB2);
            var area2 = SignedTriangleArea(lineA1, lineA2, lineB1);

            if (area1 * area2 < 0)
            {
                var area3 = SignedTriangleArea(lineB1, lineB2, lineA1);
                var area4 = area3 + area2 - area1;
                if (area3 * area4 < 0)
                {
                    var intersectionScale = area3 / (area3 - area4);
                    var intersectionPoint = (lineA2 - lineA1) * intersectionScale + lineA1;

                    // 返回交点
                    return intersectionPoint;
                }
            }

            return null;
        }

        /// <summary>
        /// 计算三角形的有向面积
        /// </summary>
        /// <param name="a">顶点A</param>
        /// <param name="b">顶点B</param>
        /// <param name="c">顶点C</param>
        /// <returns></returns>
        public static double SignedTriangleArea(Vector2D a, Vector2D b, Vector2D c)
        {
            return (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);
        }

        /// <summary>
        /// 测试一个点是否在一个多边形的内部
        /// </summary>
        /// <param name="vertexs">多边形的顺时针点集，最后一个点应当与第一个点为同一点</param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsInside(IReadOnlyList<Vector2D> vertexs, Vector2D point)
        {
            int count = 0;
            int num = vertexs.Count() - 1;

            if (num < 3)
            {
                // 当点集不能围成多边形时该函数永假
                return false;
            }

            for (int i = 0; i < num; i++)
            {
                var slope = (vertexs[i + 1].Y - vertexs[i].Y) / (vertexs[i + 1].X - vertexs[i].X);
                bool cond1 = (vertexs[i].X <= point.X) && (point.X < vertexs[i + 1].X);
                bool cond2 = (vertexs[i + 1].X <= point.X) && (point.X < vertexs[i].X);
                bool above = point.Y < slope * (point.X - vertexs[i].X) + vertexs[i].Y;
                if ((cond1 || cond2) && above)
                {
                    count++;
                }
            }

            return count % 2 != 0;
        }

        /// <summary>
        /// 计算点到直线的距离的平方
        /// </summary>
        /// <param name="point"></param>
        /// <param name="linePA"></param>
        /// <param name="linePB"></param>
        /// <returns></returns>
        public static double PointToLineDistenceSquared(Vector2D point, Vector2D linePA, Vector2D linePB)
        {
            return PointToLineVector(point, linePA, linePB).LengthSquared();
        }
    }
}
