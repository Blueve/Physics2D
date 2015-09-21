using System.Collections.Generic;

using Physics2D.Common;
using Physics2D.Object;
using static Physics2D.Collision.ParticleBall;
using static Physics2D.Common.MathHelper;

namespace Physics2D.Collision
{
    public class ParticleEdge : ParticleContactGenerator
    {
        public Vector2D PointA { get; }
        public Vector2D PointB { get; }

        public double Restitution { get; }

        private readonly List<Ball> _ballList = new List<Ball>();


        public void AddBall(Particle particle, double r)
        {
            _ballList.Add(new Ball { Particle = particle, R = r });
        }

        public ParticleEdge(double restitution, double x1, double y1, double x2, double y2)
        {
            this.Restitution = restitution;
            PointA = new Vector2D(x1, y1);
            PointB = new Vector2D(x2, y2);
        }

        public override int FillContact(List<ParticleContact> contactList, int limit)
        {
            int count = 0;
            foreach(var item in _ballList)
            {
                var intersectionPoint = LineIntersection(item.Particle.PrePosition, item.Particle.Position, PointA, PointB);
                
                // 判断物体的运动路径是否发生穿越
                if (intersectionPoint != null)
                {
                    // 发生穿越则认为发生碰撞 将质体位置退至相交点
                    item.Particle.Position = (Vector2D)intersectionPoint;
                    
                    // 产生一组碰撞
                    var BA = PointB - PointA;
                    var normal = BA * (item.Particle.PrePosition - PointA) * BA / BA.LengthSquared();
                    normal = (item.Particle.PrePosition - PointA) - normal;

                    normal.Normalize();

                    var contact = new ParticleContact
                    {
                        PA = item.Particle,
                        Restitution = Restitution,
                        Penetration = item.R,
                        ContactNormal = normal
                    };
                    // 加入碰撞列表
                    contactList.Add(contact);
                    // 计数
                    ++count;
                    continue;
                }

                // 若未发生穿越则计算
                double rd = item.R;
                double n1 = (PointA - item.Particle.Position) * (PointA - PointB);
                double n2 = (PointB - item.Particle.Position) * (PointA - PointB);

                if (n1 * n2 <= 0)
                {
                    // 线段上存在比端点到圆心距离更近的点

                    // 计算线段所处直线到圆心的距离
                    var BA = PointB - PointA;
                    var normal = BA * (item.Particle.Position - PointA) * BA / BA.LengthSquared();
                    normal = (item.Particle.Position - PointA) - normal;

                    if (rd > normal.Length())
                    {
                        // 产生一组碰撞
                        var contact = new ParticleContact
                        {
                            PA = item.Particle,
                            Restitution = Restitution,
                            Penetration = (rd - normal.Length()),
                            ContactNormal = normal.Normalize()
                        };
                        // 加入碰撞列表
                        contactList.Add(contact);
                        // 计数
                        ++count;
                    }
                }
            }
            return count;
        }
    }
}
