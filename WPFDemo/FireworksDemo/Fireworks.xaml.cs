using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.FireworksDemo
{
    /// <summary>
    /// Fireworks.xaml 的交互逻辑
    /// </summary>
    public partial class Fireworks : Window
    {
        private WPFDemo.FireworksDemo.FireworksDemo _fireworksDemo;

        public Fireworks()
        {
            InitializeComponent();
            _fireworksDemo = new WPFDemo.FireworksDemo.FireworksDemo(ImageSurface);

            ImageSurface.Source = _fireworksDemo.Bitmap;
            CompositionTarget.Rendering += _fireworksDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _fireworksDemo.Fire((float)e.GetPosition(ImageSurface).X / 50, (float)e.GetPosition(ImageSurface).Y / 50);
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (Drag.IsChecked == true)
            {
                _fireworksDemo.Type = WPFDemo.FireworksDemo.FireworksDemo.WaterG;
            }
            else if (Wind.IsChecked == true)
            {
                _fireworksDemo.Type = WPFDemo.FireworksDemo.FireworksDemo.WindG;
            }
        }
    }
}