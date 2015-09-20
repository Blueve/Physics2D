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
            int limit = Settings.MaxContacts;
            for (int i = 0; i < Settings.ContactIteration; i++)
            {
                // 产生碰撞表
                _contactList.Clear();
                foreach (var item in _registrations)
                {
                    limit -= item.FillContact(_contactList, limit);
                    if (limit <= 0) break;
                }

                // 当不再产生新的碰撞时退出
                if(_contactList.Count == 0) break;
             
                // 解决质体碰撞
                _particleContactResolver.Iterations = _contactList.Count * 2;
                _particleContactResolver.ResolveContacts(_contactList, duration);
            }

        }
    }
}
