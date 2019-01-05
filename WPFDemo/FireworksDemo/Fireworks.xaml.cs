namespace WPFDemo.FireworksDemo
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Physics2D;

    /// <summary>
    /// Fireworks.xaml 的交互逻辑
    /// </summary>
    public partial class Fireworks : Window
    {
        private FireworksDemo fireworksDemo;

        public Fireworks()
        {
            this.InitializeComponent();
            this.fireworksDemo = new FireworksDemo(this.ImageSurface);

            this.ImageSurface.Source = this.fireworksDemo.Bitmap;
            CompositionTarget.Rendering += this.fireworksDemo.Update;
        }

        private void ImageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.fireworksDemo.Fire(e.GetPosition(this.ImageSurface).X.ToSimUnits(), e.GetPosition(this.ImageSurface).Y.ToSimUnits());
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (this.Drag.IsChecked == true)
            {
                this.fireworksDemo.Type = FireworksDemo.PhysicsType.Water;
            }
            else if (this.Wind.IsChecked == true)
            {
                this.fireworksDemo.Type = FireworksDemo.PhysicsType.Wind;
            }
        }
    }
}