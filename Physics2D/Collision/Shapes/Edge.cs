using Physics2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision.Shapes
{
    public sealed class Edge : Shape
    {
        public Vector2D PointA;
        public Vector2D PointB;

        public Edge(Vector2D pA, Vector2D pB)
        {
            PointA = pA;
            PointB = pB;
        }

        public Edge(double x1, double y1, double x2, double y2)
        {
            PointA = new Vector2D(x1, y1);
            PointB = new Vector2D(x2, y2);
        }

        public override ShapeType Type
        {
            get { return ShapeType.Edge; }
        }
    }
}
