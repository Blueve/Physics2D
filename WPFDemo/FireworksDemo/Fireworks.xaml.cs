using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo
{
    /// <summary>
    /// Fireworks.xaml 的交互逻辑
    /// </summary>
    public partial class Fireworks : Window
    {
        private FireworksDemo.FireworksDemo _fireworksDemo;

        public Fireworks()
        {
            InitializeComponent();
            _fireworksDemo = new FireworksDemo.FireworksDemo(ImageSurface);

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
                _fireworksDemo.Type = FireworksDemo.FireworksDemo.WaterG;
            }
            else if (Wind.IsChecked == true)
            {
                _fireworksDemo.Type = FireworksDemo.FireworksDemo.WindG;
            }
        }
    }
}