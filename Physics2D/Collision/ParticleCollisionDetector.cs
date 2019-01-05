namespace Physics2D.Collision
{
    using System;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;

    public class ParticleCollisionDetector
    {
        /// <summary>
        /// 检测圆形与圆形间的碰撞
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static ParticleContact CircleAndCircle(Circle A, Circle B)
        {
            var d = (A.Body.Position - B.Body.Position).Length();

            // 碰撞检测
            var l = A.R + B.R;

            if (d >= l)
            {
                return null;
            }

            // 产生一组碰撞
            return new ParticleContact(
                A.Body, B.Body,
                (A.Body.Restitution + B.Body.Restitution) / 2,
                l - d,
                (A.Body.Position - B.Body.Position).Normalize());
        }

        /// <summary>
        /// 检测圆形与边缘的碰撞
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="edge"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static ParticleContact CircleAndEdge(Circle circle, Edge edge)
        {
            var BA = edge.PointB - edge.PointA;
            var BALengthSquared = BA.LengthSquared();

            var intersectionPoint = MathHelper.LineIntersection(
                circle.Body.PrePosition,
                circle.Body.Position,
                edge.PointA,
                edge.PointB);

            // 检测物体的运动路径是否发生穿越
            if (intersectionPoint != null)
            {
                // 发生穿越则认为发生碰撞 将质体位置退至相交点
                circle.Body.Position = (Vector2D)intersectionPoint;
            }

            // 若未发生穿越则计算
            double n1 = (edge.PointA - circle.Body.Position) * (edge.PointA - edge.PointB);
            double n2 = (edge.PointB - circle.Body.Position) * (edge.PointA - edge.PointB);

            if (n1 * n2 <= 0)
            {
                // 圆心的投影在线上
                var normal = BA * (circle.Body.Position - edge.PointA) * BA / BALengthSquared;
                normal = (circle.Body.Position - edge.PointA) - normal;

                // 线到圆心的距离
                var d = normal.Length();
                if (circle.R > d)
                {
                    // 针对圆心正好处于线上的情况的作处理
                    if (Math.Abs(d) < Settings.Percision)
                    {
                        normal = BA * (circle.Body.PrePosition - edge.PointA) * BA / BALengthSquared;
                        normal = (circle.Body.PrePosition - edge.PointA) - normal;
                    }

                    return new ParticleContact(
                        circle.Body, null,
                        circle.Body.Restitution,
                        circle.R - d,
                        normal.Normalize());
                }
            }
            else
            {
                // 圆心的投影在线外
                var dAO = (circle.Body.Position - edge.PointA).LengthSquared();
                var dBO = (circle.Body.Position - edge.PointB).LengthSquared();
                if (circle.R * circle.R > Math.Min(dAO, dBO))
                {
                    var normal = circle.Body.Position - (dAO < dBO ? edge.PointA : edge.PointB);
                    return new ParticleContact(
                        circle.Body, null,
                        circle.Body.Restitution,
                        circle.R - Math.Sqrt(dAO < dBO ? dAO : dBO),
                        normal.Normalize());
                }
            }

            return null;
        }
    }
}
