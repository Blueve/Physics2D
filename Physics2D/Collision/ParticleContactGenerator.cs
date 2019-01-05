namespace Physics2D.Collision
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 碰撞生成器
    /// </summary>
    public abstract class ParticleContactGenerator : IEnumerable<ParticleContact>
    {
        public abstract IEnumerator<ParticleContact> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
