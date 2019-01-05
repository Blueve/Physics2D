namespace Physics2D.Object
{
    using Physics2D.Common;

    public class Particle : PhysicsObject
    {
        /// <summary>
        /// 更新质体
        /// </summary>
        /// <param name="duration"></param>
        public override void Update(double duration)
        {
            this.PrePosition = this.Position;

            // 对位置速度以及加速度进行更新
            this.Acceleration = this.forceAccum * this.inverseMass;

            this.Position += this.Velocity * duration;
            this.Velocity += this.Acceleration * duration;

            // 清除作用力
            this.forceAccum = Vector2D.Zero;
        }
    }
}