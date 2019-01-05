namespace Physics2D.Collision.Shapes
{
    public sealed class Circle : Shape
    {
        public double R;

        public Circle(double r, int id = 0)
        {
            this.R = r;
            this.Id = id;
        }

        public override ShapeType Type
        {
            get { return ShapeType.Circle; }
        }
    }
}
