namespace Physics2D.Force.Zones
{
    /// <summary>
    /// 能对所有物体施加作用力的区域
    /// </summary>
    public class GlobalZone : Zone
    {
        protected override bool IsIn(Object.PhysicsObject obj) => true;
    }
}