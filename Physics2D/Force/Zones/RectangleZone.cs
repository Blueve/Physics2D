using Physics2D.Object;

namespace Physics2D.Force.Zones
{
    /// <summary>
    /// 在矩形内施加作用力的区域
    /// </summary>
    public class RectangleZone : Zone
    {
        private double x1;
        private double y1;
        private double x2;
        private double y2;

        public RectangleZone(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        protected override bool IsIn(PhysicsObject obj)
        {
            if (obj.Position.X > x1 && obj.Position.X < x2 &&
                obj.Position.Y > y1 && obj.Position.Y < y2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}