using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    internal class ParticleContactResolver
    {
        /// <summary>
        /// 迭代次数
        /// </summary>
        protected int iterations;

        /// <summary>
        /// 已经迭代的次数
        /// </summary>
        protected int iterationsUsed;

        public ParticleContactResolver(int iterations)
        {
            this.iterations = iterations;
        }

        public void resolveContacts(List<ParticleContact> contactList, float duration)
        {
            iterationsUsed = 0;
            while(iterationsUsed++ < iterations)
            {
                // 找到分离速度最大的一组碰撞 优先处理
                float max = 0f;
                int maxI = contactList.Count;
                for(int i = 0; i < contactList.Count; i++)
                {
                    // 计算分离速度
                    float sepV = contactList[i].calculateSeparatingVelocity();
                    if(sepV < max)
                    {
                        max = sepV;
                        maxI = i;
                    }
                }
                // 解决碰撞
                contactList[maxI].resolve(duration);
            }
        }

        public int Iterations
        {
            get { return iterations; }
            set { iterations = value; }
        }
    }
}
