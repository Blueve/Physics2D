using Physics2D.Common;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision.Shapes
{
    public sealed class Sphere<T> : Shape<T> where T : PhysicsObject
    {
        public Vector2D Position;
        public double R;
    }
}
