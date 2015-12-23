using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Physics2D.Collision;
using Physics2D;
using Physics2D.Object;
using Physics2D.Factories;

using WPFDemo.Graphic;

namespace WPFDemo.ContactDemo
{
    class Ball : IDrawable
    {
        #region 公开属性
        public Particle FixedParticle;
        public Particle Particle;
        public int R;
        #endregion

        #region 实现IDrawable
        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.DrawLineAa
            (
                FixedParticle.Position.X.ToDisplayUnits(),
                FixedParticle.Position.Y.ToDisplayUnits(),
                Particle.Position.X.ToDisplayUnits(),
                Particle.Position.Y.ToDisplayUnits(),
                Colors.DarkGray
            );
            bitmap.FillEllipseCentered
            (
                Particle.Position.X.ToDisplayUnits(),
                Particle.Position.Y.ToDisplayUnits(), R, R, Colors.DarkRed
            );
        }
        #endregion
    }
}
