namespace Physics2D.Object
{
    using Physics2D.Core;

    /// <summary>
    /// 自定义的物体需要实现该抽象类
    /// </summary>
    public abstract class CustomObject : PhysicsObject
    {
        /// <summary>
        /// 实现自定义的物理世界装载
        /// </summary>
        /// <param name="world"></param>
        public abstract void OnInit(World world);

        /// <summary>
        /// 实现自定义的物理世界移除
        /// </summary>
        /// <param name="world"></param>
        public abstract void OnRemove(World world);
    }
}
