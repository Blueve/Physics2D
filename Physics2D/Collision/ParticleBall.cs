using System.Collections.Generic;
using Physics2D.Object;

namespace Physics2D.Collision
{
    /// <summary>
    ///     检测球形质体和球形质体之间的碰撞
    /// </summary>
    public class ParticleBall : ParticleContactGenerator
    {
        private readonly List<Ball> _ballList = new List<Ball>();

        public ParticleBall(double restitution)
        {
            Restitution = restitution;
        }

        public double Restitution { get; set; }

        public void AddBall(Particle particle, double r)
        {
            // 添加一个球
            _ballList.Add(new Ball {Particle = particle, R = r});
        }

        public override int FillContact(List<ParticleContact> contactList, int limit)
        {
            var contactCount = 0;
            // 检查所有组合
            for (var i = 0; i < _ballList.Count; i++)
            {
                for (var j = i + 1; j < _ballList.Count; j++)
                {
                    var d = (_ballList[i].Particle.Position - _ballList[j].Particle.Position).Length();
                    // 碰撞检测
                    var l = _ballList[i].R + _ballList[j].R;
                    if (!(d < l)) continue;
                    // 产生一组碰撞
                    var contact = new ParticleContact
                    {
                        PA = _ballList[i].Particle,
                        PB = _ballList[j].Particle,
                        Restitution = Restitution,
                        Penetration = (l - d)/2,
                        ContactNormal = (_ballList[i].Particle.Position - _ballList[j].Particle.Position).Normalize()
                    };
                    // 加入碰撞列表
                    contactList.Add(contact);
                    ++contactCount;
                }
            }
            return contactCount;
        }

        internal struct Ball
        {
            public Particle Particle;
            public double R;
        }
    }
}