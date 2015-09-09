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
        protected int _iterations;

        public ParticleContactResolver(int iterations)
        {
            this._iterations = iterations;
        }

        public void ResolveContacts(List<ParticleContact> contactList, double duration)
        {
            if (contactList.Count == 0) return;
            int iterationsUsed = 0;
            while(iterationsUsed++ < _iterations)
            {
                // 找到分离速度最大的一组碰撞 优先处理
                double max = 0;
                int maxI = contactList.Count - 1;
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
                contactList[maxI].Resolve(duration);
            }
        }

        public int Iterations
        {
            get { return _iterations; }
            set { _iterations = value; }
        }
    }
}
