using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Common;
using Physics2D.Object;

namespace Physics2D.Collision
{
    public class ParticleContact
    {
        #region 公共属性
        /// <summary>
        /// 接触物体A
        /// </summary>
        public Particle PA;
        
        /// <summary>
        /// 接触物体B
        /// </summary>
        public Particle PB;

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
        #endregion

        #region 公开方法
        /// <summary>
        /// 解决碰撞问题
        /// 解决速度及相交
        /// </summary>
        /// <param name="duration">持续时间</param>
        public void Resolve(double duration)
        {
            ResolveVelocity(duration);
            ResolveInterpenetration(duration);
        }

        /// <summary>
        /// 计算分离速度
        /// </summary>
        /// <returns>分离速度</returns>
        public double CalculateSeparatingVelocity()
        {
            return (PA.Velocity - (PB?.Velocity ?? Vector2D.Zero)) * ContactNormal;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 解决碰撞后速度
        /// </summary>
        /// <param name="duration"></param>
        private void ResolveVelocity(double duration)
        {
            double separatingVelocity = CalculateSeparatingVelocity();

            if (separatingVelocity > 0)
            {
                // 两个物体已经分离或静止
                return;
            }

            double newSeparatingVelocity = -separatingVelocity * Restitution;

            // 检查仅由加速度产生的速度
            double accCausedSeparatingVelocity = (PA.Acceleration - (PB?.Acceleration ?? Vector2D.Zero)) * ContactNormal * duration;
            if (accCausedSeparatingVelocity < 0)
            {
                // 补偿由加速度产生的速度
                newSeparatingVelocity += Restitution * accCausedSeparatingVelocity;
                // 避免过度补偿
                if (newSeparatingVelocity < 0) newSeparatingVelocity = 0;
            }

            double deltaVelocity = newSeparatingVelocity - separatingVelocity;
            double totalInverseMass = PA.InverseMass + (PB?.InverseMass ?? 0);

            // 两个物体全为固定或匀速物体则不处理
            if (totalInverseMass <= 0) return;

            // 计算冲量
            double impulse = deltaVelocity / totalInverseMass;

            // 施加冲量
            var impulsePerIMass = ContactNormal * impulse;
            PA.Velocity += impulsePerIMass * PA.InverseMass;

            if (PB != null)
                PB.Velocity -= impulsePerIMass * PB.InverseMass;
            else
            {
                // 静态碰撞的处理

                // 计算PA在碰撞法线上的速度分量
                var vP = PA.Velocity * ContactNormal;
                // 计算PA加速度在未来产生的速度在碰撞法线上的分量
                var vF = PA.Acceleration * duration * ContactNormal;
                if (Math.Abs(vP + vF) < Math.Abs(vF))
                {
                    PA.Velocity -= vP * ContactNormal;
                }
            }

        }

        /// <summary>
        /// 解决碰撞后相交
        /// </summary>
        /// <param name="duration"></param>
        private void ResolveInterpenetration(double duration)
        {
            // 对象未相交
            if (Penetration <= 0) return;

            // 不处理两个均为固定或常速运动的物体
            double totalInverseMass = PA.InverseMass + (PB?.InverseMass ?? 0);
            if (totalInverseMass <= 0) return;

            var movePerIMass = ContactNormal * (Penetration / totalInverseMass);

            MovementA = PA.InverseMass * movePerIMass;
            MovementB = -PB?.InverseMass*movePerIMass ?? Vector2D.Zero;

            PA.Position += MovementA;
            if (PB != null)
                PB.Position += MovementB;
        }
        #endregion
    }
}
