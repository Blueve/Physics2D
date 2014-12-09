using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Collision
{
    /// <summary>
    /// 检测球形质体和球形质体之间的碰撞
    /// </summary>
    public class ParticleBall : ParticleContactGenerator
    {
        private List<Ball> ballList = new List<Ball>();

        private float restitution;

        public ParticleBall(float restitution)
        {
            this.restitution = restitution;
        }

        public void AddBall(Particle particle, float r)
        {
            // 添加一个球
            ballList.Add(new Ball { particle = particle, r = r });
        }

        public override int fillContact(List<ParticleContact> contactList, int limit)
        {
            if (limit == 0) return 0;

            int contactCount = 0;
            // 检查所有组合
            for(int i = 0; i < ballList.Count; i++)
            {
                for (int j = i + 1; j < ballList.Count; j++)
                {
                    float d = (ballList[i].particle.Position - ballList[j].particle.Position).Length();
                    // 碰撞检测
                    float l = ballList[i].r + ballList[j].r;
                    if(d < l)
                    {
                        // 产生一组碰撞
                        ParticleContact contact = new ParticleContact
                        {
                            PA = ballList[i].particle,
                            PB = ballList[j].particle,
                            restitution = restitution,
                            penetration = (l - d) / 2,
                            contactNormal = (ballList[i].particle.Position - ballList[j].particle.Position).Normalize()
                        };
                        // 加入碰撞列表
                        contactList.Add(contact);
                        // 计数
                        ++contactCount;
                        //if (++contactCount == limit) return limit;
                    }
                }
            }
            return contactCount;
        }

        private struct Ball
        {
            public Particle particle;
            public float r;
        }
    }
}
