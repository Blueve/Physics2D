using Physics2D.Object;

namespace Physics2D.Force.Zones
{
    /// <summary>
    /// 在矩形内施加作用力的区域
    /// </summary>
    public class RectangleZone : Zone
    {
        private float x1;
        private float y1;
        private float x2;
        private float y2;

        public RectangleZone(float x1, float y1, float x2, float y2)
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