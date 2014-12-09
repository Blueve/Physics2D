using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Common;
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
                // 半径的平方
                float rS = item.r * item.r;

                float n1 = (pointA - item.particle.Position) * (pointA - pointB);
                float n2 = (pointB - item.particle.Position) * (pointA - pointB);

                if (n1 * n2 > 0f)
                {
                    // 线段上的点到圆心的距离不大于到端点的距离

                    // 分别计算两个端点到圆心的距离的平方
                    float dAO = (item.particle.Position - pointA).Length();
                    float dBO = (item.particle.Position - pointB).Length();

                    if (item.r > dAO || item.r > dBO) count++;
                }
                else
                {
                    // 线段上存在比端点到圆心距离更近的点

                    // 计算线段所处直线到圆心的距离平方
                    float A = pointB.Y - pointA.Y;
                    float B = -(pointB.X - pointA.X);
                    float C = (pointB.X - pointA.X) * pointA.Y - A * pointA.X;
                    float tmp = Math.Abs(A * item.particle.Position.X + B * item.particle.Position.Y + C);

                    float dAB = tmp / (float)Math.Sqrt(A * A + B * B);

                    if (item.r > dAB)
                    {
                        var l = pointB - pointA;
                        var normal = (pointB - pointA) * (item.particle.Position - pointA) * l.Normalize();
                        normal = (item.particle.Position - pointA) - normal;
                        normal = new Vector2D(0f, -1f);
                        // 产生一组碰撞
                        ParticleContact contact = new ParticleContact
                        {
                            PA = item.particle,
                            restitution = restitution,
                            penetration = (item.r - dAB),
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
