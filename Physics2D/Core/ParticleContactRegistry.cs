using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Collision;

namespace Physics2D.Core
{
    public sealed class ParticleContactRegistry
    {
        /// <summary>
        /// 质体碰撞发生器集合
        /// </summary>
        private readonly HashSet<ParticleContactGenerator> _registrations = new HashSet<ParticleContactGenerator>();

        /// <summary>
        /// 碰撞表
        /// </summary>
        private readonly List<ParticleContact> _contactList = new List<ParticleContact>();

        /// <summary>
        /// 碰撞解决器
        /// </summary>
        private readonly ParticleContactResolver _particleContactResolver = new ParticleContactResolver(0);

        private int _contactCounter = 0;

        /// <summary>
        /// 注册碰撞发生器
        /// </summary>
        /// <param name="contactGenerator"></param>
        public void Add(ParticleContactGenerator contactGenerator) => _registrations.Add(contactGenerator);

        /// <summary>
        /// 移除碰撞发生器
        /// </summary>
        /// <param name="contactGenerator"></param>
        public void Remove(ParticleContactGenerator contactGenerator) => _registrations.Remove(contactGenerator);

        /// <summary>
        /// 进行碰撞检测并解决碰撞
        /// </summary>
        /// <param name="duration"></param>
        public void ResolveContacts(double duration)
        {
            _contactCounter = 0;
            for (int i = 0; i < Settings.ContactIteration; i++)
            {
                // 产生碰撞表
                _contactList.Clear();
                foreach (var contactGenerator in _registrations)
                {
                    foreach (var contact in contactGenerator)
                    {
                        if (!AddToContactList(contact)) goto CONTACT_RESOLVE;
                    }
                }

                // 当不再产生新的碰撞时退出
                if (_contactList.Count == 0) break;
                
                CONTACT_RESOLVE:
                // 解决质体碰撞
                _particleContactResolver.Iterations = _contactList.Count * 2;
                _particleContactResolver.ResolveContacts(_contactList, duration);
            }

        }

        /// <summary>
        /// 向碰撞表中添加一个新的碰撞
        /// </summary>
        /// <param name="contact">碰撞信息</param>
        /// <returns>完成添加后若不允许继续添加则返回false，否则返回true</returns>
        private bool AddToContactList(ParticleContact contact)
        {
            if (_contactCounter++ < Settings.MaxContacts)
            {
                _contactList.Add(contact);
                return true;
            }
            return false;
        }
    }
}
