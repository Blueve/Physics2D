using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    /// <summary>
    /// 碰撞生成器
    /// </summary>
    public abstract class ParticleContactGenerator
    {
        public abstract int FillContact(List<ParticleContact> contactList, int limit);
    }
}
