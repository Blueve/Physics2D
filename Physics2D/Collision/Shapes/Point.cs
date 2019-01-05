namespace Physics2D.Collision.Shapes
{
    /// <summary>
    /// 点形状适用于无形体的物体，通常将其作为物体的默认形状
    /// 点状物体不参与碰撞检测
    /// </summary>
    public sealed class Point : Shape
    {
        public override ShapeType Type
        {
            get { return ShapeType.Point; }
        }
    }
}
