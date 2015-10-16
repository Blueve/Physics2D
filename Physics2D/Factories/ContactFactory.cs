using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Collision.Basic;
using Physics2D.Core;

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
            world.ContactGenerators.Add(edge);
            return edge;
        }


        #endregion
    }
}
