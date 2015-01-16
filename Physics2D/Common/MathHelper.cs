using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Common.Exceptions;

namespace Physics2D.Common
{
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
            Vector2D BA = linePB - linePA;
            Vector2D normal = BA * (point - linePA) * BA / BA.LengthSquared() - (point - linePA);

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
        public static Vector2D LineIntersection(Vector2D lineA1, Vector2D lineA2, Vector2D lineB1, Vector2D lineB2)
        {
            var area1 = SignedTriangleArea(lineA1, lineA2, lineB2);
            var area2 = SignedTriangleArea(lineA1, lineA2, lineB1);

            if(area1 * area2 < 0f)
            {
                var area3 = SignedTriangleArea(lineB1, lineB2, lineA1);
                var area4 = area3 + area2 - area1;
                if(area3 * area4 < 0f)
                {
                    var intersectionScale = area3 / (area3 - area4);
                    var intersectionPoint = (lineA2 - lineA1) * intersectionScale + lineA1;

                    // 返回交点
                    return intersectionPoint;
                }
            }
            throw new PointNotFoundException("");
        }

        public static bool IsLineIntersection(Vector2D lineA1, Vector2D lineA2, Vector2D lineB1, Vector2D lineB2)
        {
            var area1 = SignedTriangleArea(lineA1, lineA2, lineB2);
            var area2 = SignedTriangleArea(lineA1, lineA2, lineB1);

            if (area1 * area2 < 0f)
            {
                var area3 = SignedTriangleArea(lineB1, lineB2, lineA1);
                var area4 = area3 + area2 - area1;
                if (area3 * area4 < 0f)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算三角形的有向面积
        /// </summary>
        /// <param name="a">顶点A</param>
        /// <param name="b">顶点B</param>
        /// <param name="c">顶点C</param>
        /// <returns></returns>
        public static float SignedTriangleArea(Vector2D a, Vector2D b, Vector2D c)
        {
            float result = (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);
            return result;
        }
    }
}
