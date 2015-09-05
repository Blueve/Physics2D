using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Object;

namespace Physics2D.Collision
{
    public abstract class ParticleLink : ParticleContactGenerator
    {
        /// <summary>
        /// 质体A
        /// </summary>
        public Particle PA;
        /// <summary>
        /// 质体B
        /// </summary>
        public Particle PB;

        /// <summary>
        /// 返回当前连接的长度
        /// </summary>
        /// <returns></returns>
        protected double currentLength()
        {
            return (PA.Position - PB.Position).Length();
        }
    }
}
