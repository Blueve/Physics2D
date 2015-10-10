using System.Windows.Media.Imaging;
using SharpDX.Direct2D1;

namespace WPFDemo.Graphic
{
    public interface IDrawable
    {
        void Draw(WriteableBitmap bitmap);
    }

    public interface IRenderable
    {
        void Render(RenderTarget target);
    }

}