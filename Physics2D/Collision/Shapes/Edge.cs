namespace Physics2D.Collision.Shapes
{
    using Physics2D.Common;

    public sealed class Edge : Shape
    {
        public Vector2D PointA;
        public Vector2D PointB;

        public Edge(Vector2D pA, Vector2D pB)
        {
            this.PointA = pA;
            this.PointB = pB;
        }

        public Edge(double x1, double y1, double x2, double y2)
        {
            this.PointA = new Vector2D(x1, y1);
            this.PointB = new Vector2D(x2, y2);
        }

        public override ShapeType Type
        {
            get { return ShapeType.Edge; }
        }
    }
}
