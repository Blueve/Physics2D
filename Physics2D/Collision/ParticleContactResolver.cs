using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    internal class ParticleContactResolver
    {
        public ParticleContactResolver(int iterations)
        {
            Iterations = iterations;
        }

        public void ResolveContacts(List<ParticleContact> contactList, double duration)
        {
            if (contactList.Count == 0) return;
            int iterationsUsed = 0;
            while(iterationsUsed++ < Iterations)
            {
                // 找到分离速度最大的一组碰撞 优先处理
                double max = 0;
                int maxI = contactList.Count;
                for(int i = 0; i < contactList.Count; i++)
                {
                    // 计算分离速度
                    double sepV = contactList[i].CalculateSeparatingVelocity();
                    if(sepV < max)
                    {
                        max = sepV;
                        maxI = i;
                    }
                }

                // 解决碰撞
                if (maxI == contactList.Count) break;
                contactList[maxI].Resolve(duration);

                // 更新相交长度
                var maxItem = contactList[maxI];
                var movementA = contactList[maxI].MovementA;
                var movementB = contactList[maxI].MovementB;
                contactList.ForEach(item =>
                {
                    if (item.PA == maxItem.PA)
                    {
                        item.Penetration -= movementA * item.ContactNormal;
                    }
                    else if (item.PA == maxItem.PB)
                    {
                        item.Penetration -= movementB*item.ContactNormal;
                    }
                    if (item.PB != null)
                    {
                        if (item.PB == maxItem.PA)
                        {
                            item.Penetration += movementA * item.ContactNormal;
                        }
                        else if (item.PB == maxItem.PB)
                        {
                            item.Penetration += movementB * item.ContactNormal;
                        }
                    }
                });
            }
        }

        public int Iterations { get; set; }
    }
}
