using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Common;
using Physics2D.Common.Exceptions;
using Physics2D.Object;

namespace Physics2D.Collision
{
    public class ParticleEdge : ParticleContactGenerator
    {
        private Vector2D pointA;
        private Vector2D pointB;

        private float restitution;

        private List<ParticleBall.Ball> ballList = new List<ParticleBall.Ball>();


        public void AddBall(Particle particle, float r)
        {
            // 添加一个球
            ballList.Add(new ParticleBall.Ball { particle = particle, r = r });
        }

        public ParticleEdge(float restitution, float x1, float y1, float x2, float y2)
        {
            this.restitution = restitution;
            pointA = new Vector2D(x1, y1);
            pointB = new Vector2D(x2, y2);
        }

        public override int fillContact(List<ParticleContact> contactList, int limit)
        {
            int count = 0;
            foreach(var item in ballList)
            {
                // 判断物体的运动路径是否发生穿越
                if (MathHelper.IsLineIntersection(item.particle.PrePosition, item.particle.Position, pointA, pointB))
                {
                    var intersectionPoint = MathHelper.LineIntersection(item.particle.PrePosition, item.particle.Position, pointA, pointB);

                    // 发生穿越则认为发生碰撞 将质体位置退至相交点
                    item.particle.Position = intersectionPoint;

                    // 产生一组碰撞
                    Vector2D BA = pointB - pointA;
                    Vector2D normal = BA * (item.particle.PrePosition - pointA) * BA / BA.LengthSquared();
                    normal = (item.particle.PrePosition - pointA) - normal;

                    ParticleContact contact = new ParticleContact
                    {
                        PA = item.particle,
                        restitution = restitution,
                        penetration = item.r,
                        contactNormal = normal
                    };
                    // 加入碰撞列表
                    contactList.Add(contact);
                    // 计数
                    ++count;
                    continue;
                }

                // 若未发生穿越则计算
                float rd = item.r;

                float n1 = (pointA - item.particle.Position) * (pointA - pointB);
                float n2 = (pointB - item.particle.Position) * (pointA - pointB);

                if (n1 * n2 > 0f)
                {
                    // 线段上的点到圆心的距离不大于到端点的距离

                    // 分别计算两个端点到圆心的距离的平方
                    float dAO = (item.particle.Position - pointA).Length();
                    float dBO = (item.particle.Position - pointB).Length();
                    if (rd > dAO || rd > dBO)
                    {
                        // 计数
                        ++count;
                    }
                }
                else
                {
                    // 线段上存在比端点到圆心距离更近的点

                    // 计算线段所处直线到圆心的距离
                    Vector2D BA = pointB - pointA;
                    Vector2D normal = BA * (item.particle.Position - pointA) * BA / BA.LengthSquared();
                    normal = (item.particle.Position - pointA) - normal;

                    if (rd > normal.Length())
                    {
                        // 产生一组碰撞
                        ParticleContact contact = new ParticleContact
                        {
                            PA = item.particle,
                            restitution = restitution,
                            penetration = (rd - normal.Length()),
                            contactNormal = normal
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
