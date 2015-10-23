using Physics2D.Common;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision.Shapes
{
    public sealed class Circle : Shape
    {
        public double R;

        public Circle(double r, int id = 0)
        {
            R = r;
            Id = id;
        }

        public override ShapeType Type
        {
            get { return ShapeType.Circle; }
        }
    }
}
