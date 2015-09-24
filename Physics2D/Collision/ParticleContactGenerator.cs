using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    /// <summary>
    /// 碰撞生成器
    /// </summary>
    public abstract class ParticleContactGenerator : IEnumerable<ParticleContact>
    {
        public abstract IEnumerator<ParticleContact> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
