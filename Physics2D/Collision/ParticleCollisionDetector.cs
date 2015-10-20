using Physics2D.Collision.Shapes;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    public class ParticleCollisionDetector
    {
        public int CircleAndCircle(Circle A, Circle B, out ParticleContact contact)
        {
            contact = null;

            var d = (A.body.Position - B.body.Position).Length();
            // 碰撞检测
            var l = A.R + B.R;

            if (!(d < l)) return 0;
            // 产生一组碰撞
            contact = new ParticleContact
            {
                PA = A.body,
                PB = B.body,
                Restitution = 1,
                Penetration = (l - d) / 2,
                ContactNormal = (A.body.Position - B.body.Position).Normalize()
            };
            return 1;
        }
    }
}
