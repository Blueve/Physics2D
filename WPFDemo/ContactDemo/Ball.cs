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
        public Particle fixedParticle;
        public Particle particle;
        public int r;

        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.DrawLineAa
            (
                fixedParticle.Position.X.ToDisplayUnits(),
                fixedParticle.Position.Y.ToDisplayUnits(),
                particle.Position.X.ToDisplayUnits(),
                particle.Position.Y.ToDisplayUnits(),
                Colors.DarkGray
            );
            bitmap.FillEllipseCentered
            (
                particle.Position.X.ToDisplayUnits(),
                particle.Position.Y.ToDisplayUnits(), r, r, Colors.DarkRed
            );
        }
    }
}
