namespace Physics2D.Collision
{
    using System;
    using Physics2D.Common;
    using Physics2D.Object;

    public class ParticleContact
    {
        /// <summary>
        /// 接触物体A
        /// </summary>
        public PhysicsObject PA;

        /// <summary>
        /// 接触物体B
        /// </summary>
        public PhysicsObject PB;

        /// <summary>
        /// 碰撞恢复系数
        /// </summary>
        public double Restitution;

        /// <summary>
        /// 相交深度
        /// </summary>
        public double Penetration;

        /// <summary>
        /// 碰撞法线
        /// </summary>
        public Vector2D ContactNormal;

        /// <summary>
        /// 物体A的移动
        /// </summary>
        public Vector2D MovementA;

        /// <summary>
        /// 物体B的移动
        /// </summary>
        public Vector2D MovementB;

        public ParticleContact(
            PhysicsObject a, PhysicsObject b,
            double restitution,
            double penetration,
            Vector2D contactNormal)
        {
            this.PA = a;
            this.PB = b;
            this.Restitution = restitution;
            this.Penetration = penetration;
            this.ContactNormal = contactNormal;
            this.MovementA = this.MovementB = Vector2D.Zero;
        }

        /// <summary>
        /// 解决碰撞问题
        /// 解决速度及相交
        /// </summary>
        /// <param name="duration">持续时间</param>
        public void Resolve(double duration)
        {
            this.ResolveInterpenetration(duration);
            this.ResolveVelocity(duration);
        }

        /// <summary>
        /// 计算分离速度
        /// </summary>
        /// <returns>分离速度</returns>
        public double CalculateSeparatingVelocity()
        {
            return (this.PA.Velocity - (this.PB?.Velocity ?? Vector2D.Zero)) * this.ContactNormal;
        }

        /// <summary>
        /// 解决碰撞后速度
        /// </summary>
        /// <param name="duration"></param>
        private void ResolveVelocity(double duration)
        {
            double separatingVelocity = this.CalculateSeparatingVelocity();

            if (separatingVelocity > 0)
            {
                // 两个物体正在分离
                return;
            }

            double newSeparatingVelocity = -separatingVelocity * this.Restitution;

            // 检查仅由加速度产生的速度
            double accCausedSeparatingVelocity = (this.PA.Acceleration - (this.PB?.Acceleration ?? Vector2D.Zero)) * this.ContactNormal * duration;
            if (accCausedSeparatingVelocity < 0)
            {
                // 补偿由加速度产生的速度
                newSeparatingVelocity += this.Restitution * accCausedSeparatingVelocity;

                // 避免过度补偿
                if (newSeparatingVelocity < 0)
                {
                    newSeparatingVelocity = 0;
                }
            }

            double deltaVelocity = newSeparatingVelocity - separatingVelocity;
            double totalInverseMass = this.PA.InverseMass + (this.PB?.InverseMass ?? 0);

            // 两个物体全为固定或匀速物体则不处理
            if (totalInverseMass <= 0)
            {
                return;
            }

            // 计算冲量
            double impulse = deltaVelocity / totalInverseMass;

            // 施加冲量
            var impulsePerIMass = this.ContactNormal * impulse;
            this.PA.Velocity += impulsePerIMass * this.PA.InverseMass;

            if (this.PB != null)
                this.PB.Velocity -= impulsePerIMass * this.PB.InverseMass;
            else
            {
                // 静态碰撞的处理

                // 计算PA在碰撞法线上的速度分量
                //var vP = PA.Velocity * ContactNormal;
                //// 计算PA加速度在未来产生的速度在碰撞法线上的分量
                //var vF = PA.Acceleration * duration * ContactNormal;
                //if (Math.Abs(vP + vF) < Math.Abs(vF))
                //{
                //    PA.Velocity -= vP * ContactNormal;
                //}
            }
        }

        /// <summary>
        /// 解决碰撞后相交
        /// </summary>
        /// <param name="duration"></param>
        private void ResolveInterpenetration(double duration)
        {
            // 对象未相交
            if (this.Penetration <= 0)
            {
                return;
            }

            double vA = Math.Abs(this.PA.Velocity * this.ContactNormal);
            double vB = Math.Abs(this.PB?.Velocity * this.ContactNormal ?? .0);
            var totalVec = vA + vB;

            // 不处理两个均为固定或常速运动的物体
            double totalInverseMass = this.PA.InverseMass + (this.PB?.InverseMass ?? 0);
            if (totalInverseMass <= 0)
            {
                return;
            }

            // 两质体速度和不为0时根据速度将物体分离
            if (Math.Abs(totalVec) > Settings.Percision)
            {
                var totalInverseVec = 1 / totalVec;
                this.MovementA = this.ContactNormal * this.Penetration * totalInverseVec * vA;
                this.MovementB = -this.ContactNormal * this.Penetration * totalInverseVec * vB;
            }

            // 两质体速度和为0时根据质量将物体分离
            else
            {
                var movePerIMass = this.ContactNormal * (this.Penetration / totalInverseMass);

                this.MovementA = this.PA.InverseMass * movePerIMass;
                this.MovementB = -this.PB?.InverseMass * movePerIMass ?? Vector2D.Zero;
            }

            this.PA.Position += this.MovementA;
            if (this.PB != null)
            {
                this.PB.Position += this.MovementB;
            }
        }
    }
}
