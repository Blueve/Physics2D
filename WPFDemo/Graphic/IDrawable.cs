using System.Windows.Media.Imaging;

namespace WPFDemo.Graphic
{
    public interface IDrawable
    {
        void Draw(WriteableBitmap bitmap);
    }
}