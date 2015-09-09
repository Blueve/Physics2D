using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Collision;

namespace Physics2D.Core
{
    internal class ParticleContactRegistry
    {
        /// <summary>
        /// 质体碰撞发生器集合
        /// </summary>
        private readonly List<ParticleContactGenerator> _registrations = new List<ParticleContactGenerator>();

        /// <summary>
        /// 碰撞表
        /// </summary>
        private readonly List<ParticleContact> _contactList = new List<ParticleContact>();

        /// <summary>
        /// 碰撞解决器
        /// </summary>
        private readonly ParticleContactResolver _particleContactResolver = new ParticleContactResolver(0);

        /// <summary>
        /// 注册碰撞发生器
        /// </summary>
        /// <param name="contactGenerator"></param>
        public void Add(ParticleContactGenerator contactGenerator)
        {
            if (!_registrations.Contains(contactGenerator))
                _registrations.Add(contactGenerator);
        }

        /// <summary>
        /// 移除碰撞发生器
        /// </summary>
        /// <param name="contactGenerator"></param>
        public void Remove(ParticleContactGenerator contactGenerator)
        {
            if (_registrations.Contains(contactGenerator))
                _registrations.Remove(contactGenerator);
        }

        public void ResolveContacts(double duration)
        {
            // 产生碰撞表
            _contactList.Clear();
            int limit = Settings.MaxContacts;
            foreach (int used in _registrations.Select(item => item.FillContact(_contactList, limit)))
            {
                limit -= used;
                if (limit <= 0) break;
            }
            // 解决质体碰撞
            _particleContactResolver.Iterations = (Settings.MaxContacts - limit) * 2;
            _particleContactResolver.ResolveContacts(_contactList, duration);
        }
    }
}
