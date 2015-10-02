using Physics2D.Object;

namespace Physics2D.Force.Zones
{
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
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        protected override bool IsIn(PhysicsObject obj)
        {
            return obj.Position.X > X1 && obj.Position.X < X2 &&
                   obj.Position.Y > Y1 && obj.Position.Y < Y2;
        }
    }
}