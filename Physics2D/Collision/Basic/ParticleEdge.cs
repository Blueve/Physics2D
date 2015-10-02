using System;
using System.Collections.Generic;
using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Collision.Basic
{
    public class ParticleEdge : ParticleContactGenerator
    {
        public Vector2D PointA { get; }
        public Vector2D PointB { get; }

        public double Restitution { get; }

        private readonly List<ParticleBall.Ball> _ballList = new List<ParticleBall.Ball>();


        public void AddBall(Particle particle, double r)
        {
            _ballList.Add(new ParticleBall.Ball { Particle = particle, R = r });
        }

        public ParticleEdge(double restitution, double x1, double y1, double x2, double y2)
        {
            Restitution = restitution;
            PointA = new Vector2D(x1, y1);
            PointB = new Vector2D(x2, y2);
        }

        public override IEnumerator<ParticleContact> GetEnumerator()
        {
            var BA = PointB - PointA;

            foreach (var item in _ballList)
            {
                var intersectionPoint = MathHelper.LineIntersection(item.Particle.PrePosition, item.Particle.Position, PointA, PointB);

                // 检测物体的运动路径是否发生穿越
                if (intersectionPoint != null)
                {
                    // 发生穿越则认为发生碰撞 将质体位置退至相交点
                    item.Particle.Position = (Vector2D)intersectionPoint;
                    var normal = BA * (item.Particle.PrePosition - PointA) * BA / BA.LengthSquared();
                    normal = (item.Particle.PrePosition - PointA) - normal;

                    yield return new ParticleContact
                    {
                        PA = item.Particle,
                        Restitution = Restitution,
                        Penetration = item.R,
                        ContactNormal = normal.Normalize()
                    };
                    continue;
                }

                // 若未发生穿越则计算
                double n1 = (PointA - item.Particle.Position) * (PointA - PointB);
                double n2 = (PointB - item.Particle.Position) * (PointA - PointB);

                if (n1 * n2 <= 0)
                {
                    // 圆心的投影在线上
                    var normal = BA * (item.Particle.Position - PointA) * BA / BA.LengthSquared();
                    normal = (item.Particle.Position - PointA) - normal;
                    // 线到圆心的距离
                    var d = normal.Length();
                    if (item.R > d)
                    {
                        yield return new ParticleContact
                        {
                            PA = item.Particle,
                            Restitution = Restitution,
                            Penetration = item.R - d,
                            ContactNormal = normal.Normalize()
                        };
                    }
                }
                else
                {
                    // 圆心的投影在线外
                    var dAO = (item.Particle.Position - PointA).LengthSquared();
                    var dBO = (item.Particle.Position - PointB).LengthSquared();
                    if (item.R * item.R > Math.Min(dAO, dBO))
                    {
                        yield return new ParticleContact
                        {
                            PA = item.Particle,
                            Restitution = Restitution,
                            Penetration = item.R - Math.Sqrt(dAO < dBO ? dAO : dBO),
                            ContactNormal = item.Particle.Position - (dAO < dBO ? PointA : PointB)
                        };
                    }
                }
            }
        }
    }
}
