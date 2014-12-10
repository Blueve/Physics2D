using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="linaA1"></param>
        /// <param name="lineA2"></param>
        /// <param name="lineB1"></param>
        /// <param name="lineB2"></param>
        /// <returns></returns>
        public static Vector2D LineIntersection(Vector2D linaA1, Vector2D lineA2, Vector2D lineB1, Vector2D lineB2)
        {
            float t = ((linaA1.X - lineB1.X) * (lineB1.Y - lineA2.Y) - (linaA1.Y - lineB1.Y) * (lineB1.X - lineB2.X))
                    / ((linaA1.X - lineA2.X) * (lineB1.Y - lineB2.Y) - (linaA1.Y - lineA2.Y) * (lineB1.X - lineB2.X));
            return new Vector2D(linaA1.X + (lineA2.X - linaA1.X) * t, linaA1.Y + (lineA2.X - linaA1.Y) * t);
        }
    }
}
