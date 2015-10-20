using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision.Shapes
{
    public abstract class Shape
    {
        public PhysicsObject Body;

        public abstract ShapeType Type
        {
            get;
        }
    }
}
