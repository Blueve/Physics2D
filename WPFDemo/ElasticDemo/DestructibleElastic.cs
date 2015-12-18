using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Object;
using System.Collections.Generic;

namespace WPFDemo.ElasticDemo
{
    internal sealed class DestructibleElastic : ParticleForceGenerator
    {
        #region 私有字段
        /// <summary>
        /// 所有链接
        /// </summary>
        private readonly List<LinkedItem> _linked = new List<LinkedItem>();

        /// <summary>
        /// 弹性常量
        /// </summary>
        private readonly double _k;

        /// <summary>
        /// 静息长度
        /// </summary>
        private readonly double _length;

        /// <summary>
        /// 延展系数
        /// 当链接的长度与静息长度的比值超过该数值的时发生断裂
        /// </summary>
        private const double LengthFactor = 1.5;
        #endregion

        #region 构造方法
        public DestructibleElastic(double k, double length)
        {
            _k = k;
            _length = length;
        }
        #endregion

        #region 实现ParticleForceGenerator
        public void Joint(Particle item)
        {
            _linked.Add(new LinkedItem
            {
                Particle = item,
                IsValid = true
            });
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            for (int i = _linked.Count - 1; i >= 0; i--)
            {
                if (_linked[i].IsValid)
                {
                    var d = particle.Position - _linked[i].Particle.Position;
                    // TODO: 链接破坏规则需要修正
                    if (d.Length() > LengthFactor)
                    {
                        _linked[i].IsValid = false;
                        continue;
                    }
                    double force = (_length - d.Length()) * _k;
                    particle.AddForce(d.Normalize() * force);
                }
            }
        }
        #endregion

        #region 私有内部类
        private class LinkedItem
        {
            public Particle Particle;
            public bool IsValid;
        }
        #endregion
    }
}