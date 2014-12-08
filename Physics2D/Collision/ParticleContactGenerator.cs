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
        public abstract int addContact(List<ParticleContact> contactList, int limit);
    }
}
