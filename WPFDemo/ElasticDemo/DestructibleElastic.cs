namespace WPFDemo.ElasticDemo
{
    using System.Collections.Generic;
    using Physics2D.Force;
    using Physics2D.Object;

    internal sealed class DestructibleElastic : ParticleForceGenerator
    {
        /// <summary>
        /// 所有链接
        /// </summary>
        private readonly List<LinkedItem> linked = new List<LinkedItem>();

        /// <summary>
        /// 弹性常量
        /// </summary>
        private readonly double k;

        /// <summary>
        /// 静息长度
        /// </summary>
        private readonly double length;

        /// <summary>
        /// 延展系数
        /// 当链接的长度与静息长度的比值超过该数值的时发生断裂
        /// </summary>
        private readonly double lengthFactor;

        public DestructibleElastic(double k, double length, double lengthFactor = 4)
        {
            this.k = k;
            this.length = length;
            this.lengthFactor = lengthFactor;
        }

        public void Joint(Particle item)
        {
            this.linked.Add(new LinkedItem
            {
                Particle = item,
                IsValid = true
            });
        }

        public override void ApplyTo(Particle particle, double duration)
        {
            for (int i = this.linked.Count - 1; i >= 0; i--)
            {
                if (this.linked[i].IsValid)
                {
                    var d = particle.Position - this.linked[i].Particle.Position;
                    if (d.Length() / this.length > this.lengthFactor)
                    {
                        this.linked[i].IsValid = false;
                        continue;
                    }

                    double force = (this.length - d.Length()) * this.k;
                    particle.AddForce(d.Normalize() * force);
                }
            }
        }

        private class LinkedItem
        {
            public Particle Particle;
            public bool IsValid;
        }
    }
}