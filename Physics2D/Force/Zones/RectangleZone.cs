namespace Physics2D.Force.Zones
{
    using Physics2D.Object;

    /// <summary>
    /// 在矩形内施加作用力的区域
    /// </summary>
    public class RectangleZone : Zone
    {
        public double X1 { get; }
        public double Y1 { get; }
        public double X2 { get; }
        public double Y2 { get; }

        public RectangleZone(double x1, double y1, double x2, double y2)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }

        protected override bool IsIn(PhysicsObject obj)
        {
            return obj.Position.X > this.X1 && obj.Position.X < this.X2 &&
                   obj.Position.Y > this.Y1 && obj.Position.Y < this.Y2;
        }
    }
}