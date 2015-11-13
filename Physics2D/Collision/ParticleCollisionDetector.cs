using Physics2D.Collision.Shapes;
using Physics2D.Common;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    public class ParticleCollisionDetector
    {
        /// <summary>
        /// 检测圆形与圆形间的碰撞
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static int CircleAndCircle(Circle A, Circle B, out ParticleContact contact)
        {
            contact = null;
            if (((Particle)A.Body).IsTransparent || ((Particle)B.Body).IsTransparent)
                return 0;

            var d = (A.Body.Position - B.Body.Position).Length();
            // 碰撞检测
            var l = A.R + B.R;

            if (d >= l) return 0;
            // 产生一组碰撞
            contact = new ParticleContact(
                A.Body, B.Body, 
                (A.Body.Restitution + B.Body.Restitution) / 2, 
                (l - d) / 2, 
                (A.Body.Position - B.Body.Position).Normalize());
            return 1;
        }

        /// <summary>
        /// 检测圆形与边缘的碰撞
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="edge"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static int CircleAndEdge(Circle circle, Edge edge, out ParticleContact contact)
        {
            contact = null;

            var BA = edge.PointB - edge.PointA;
            var intersectionPoint = MathHelper.LineIntersection(circle.Body.PrePosition, circle.Body.Position, edge.PointA, edge.PointB);

            // 检测物体的运动路径是否发生穿越
            if (intersectionPoint != null)
            {
                // 发生穿越则认为发生碰撞 将质体位置退至相交点
                circle.Body.Position = (Vector2D)intersectionPoint;
                var normal = BA * (circle.Body.PrePosition - edge.PointA) * BA / BA.LengthSquared();
                normal = (circle.Body.PrePosition - edge.PointA) - normal;

                contact = new ParticleContact(circle.Body, null, circle.Body.Restitution, circle.R, normal.Normalize());
                return 1;
            }

            // 若未发生穿越则计算
            double n1 = (edge.PointA - circle.Body.Position) * (edge.PointA - edge.PointB);
            double n2 = (edge.PointB - circle.Body.Position) * (edge.PointA - edge.PointB);

            if (n1 * n2 <= 0)
            {
                // 圆心的投影在线上
                var normal = BA * (circle.Body.Position - edge.PointA) * BA / BA.LengthSquared();
                normal = (circle.Body.Position - edge.PointA) - normal;
                // 线到圆心的距离
                var d = normal.Length();
                if (circle.R > d)
                {
                    contact = new ParticleContact(circle.Body, null, circle.Body.Restitution, circle.R - d, normal.Normalize());
                    return 1;
                }
                
            }
            else
            {
                // 圆心的投影在线外
                var dAO = (circle.Body.Position - edge.PointA).LengthSquared();
                var dBO = (circle.Body.Position - edge.PointB).LengthSquared();
                if (circle.R * circle.R > Math.Min(dAO, dBO))
                {
                    contact = new ParticleContact(
                        circle.Body, null, 
                        circle.Body.Restitution, 
                        circle.R - Math.Sqrt(dAO < dBO ? dAO : dBO), 
                        circle.Body.Position - (dAO < dBO ? edge.PointA : edge.PointB));
                    return 1;
                }
            }
            
            return 0;
        }
    }
}
