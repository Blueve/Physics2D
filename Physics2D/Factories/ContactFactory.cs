using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Collision;
using Physics2D.Collision.Basic;
using Physics2D.Core;
using Physics2D.Object;

namespace Physics2D.Factories
{
    public static class ContactFactory
    {
        #region 工厂方法
        /// <summary>
        /// 在物理世界创建一条质体线段
        /// </summary>
        /// <param name="world"></param>
        /// <param name="restitution"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static ParticleEdge CreateEdge(this World world, double restitution, double x1, double y1, double x2, double y2)
        {
            var edge = new ParticleEdge(restitution, x1, y1, x2, y2);
            return world.CreateContact(edge);
        }

        /// <summary>
        /// 在物理世界创建一条质体绳索
        /// 该绳索由两个质体（质点）组成，两个质体的距离被限定在指定的数值内
        /// </summary>
        /// <param name="world"></param>
        /// <param name="maxLength"></param>
        /// <param name="restitution"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ParticleRope CreateRope(this World world, double maxLength, double restitution, Particle a, Particle b)
        {
            var rope = new ParticleRope(maxLength, restitution, a, b);
            return world.CreateContact(rope);
        }

        /// <summary>
        /// 在物理世界创建一根质体硬棒
        /// 该硬棒由两个质体（质点）组成，两个质体的距离不会发生变化
        /// </summary>
        /// <param name="world"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ParticleRod CreateRod(this World world, Particle a, Particle b)
        {
            var rod = new ParticleRod(a, b);
            return world.CreateContact(rod);
        }


        public static T CreateContact<T>(this World world, T contact)
            where T : ParticleContactGenerator
        {
            world.ContactGenerators.Add(contact);
            return contact;
        }
        #endregion
    }
}
