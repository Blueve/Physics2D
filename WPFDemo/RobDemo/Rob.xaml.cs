namespace WPFDemo.RobDemo
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Physics2D;

    /// <summary>
    /// Rob.xaml 的交互逻辑
    /// </summary>
    public partial class Rob : Window
    {
        private readonly RobDemo robDemo;

        public Rob()
        {
            this.InitializeComponent();

            this.robDemo = new RobDemo(this.ImageSurface);
            this.ImageSurface.Source = this.robDemo.Bitmap;
            CompositionTarget.Rendering += this.robDemo.Update;
        }

        private void ImageSurface_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.robDemo.Down(e.GetPosition(this.ImageSurface).X.ToSimUnits(), e.GetPosition(this.ImageSurface).Y.ToSimUnits());
        }

        private void ImageSurface_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.robDemo.Up();
        }

        private void ImageSurface_OnMouseMove(object sender, MouseEventArgs e)
        {
            this.robDemo.Move(e.GetPosition(this.ImageSurface).X.ToSimUnits(), e.GetPosition(this.ImageSurface).Y.ToSimUnits());
        }
    }
}
