namespace WPFDemo.ContactDemo
{
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Physics2D;
    using Physics2D.Object;
    using WPFDemo.Graphic;

    internal class Ball : IDrawable
    {
        public Particle FixedParticle;
        public Particle Particle;
        public int R;

        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.DrawLineAa(
                this.FixedParticle.Position.X.ToDisplayUnits(),
                this.FixedParticle.Position.Y.ToDisplayUnits(),
                this.Particle.Position.X.ToDisplayUnits(),
                this.Particle.Position.Y.ToDisplayUnits(),
                Colors.DarkGray);
            bitmap.FillEllipseCentered(
                this.Particle.Position.X.ToDisplayUnits(),
                this.Particle.Position.Y.ToDisplayUnits(), this.R, this.R, Colors.DarkRed);
        }
    }
}
