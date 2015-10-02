using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Physics2D;

namespace WPFDemo.FireworksDemo
{
    /// <summary>
    /// Fireworks.xaml 的交互逻辑
    /// </summary>
    public partial class Fireworks : Window
    {
        private FireworksDemo _fireworksDemo;

        public Fireworks()
        {
            InitializeComponent();
            _fireworksDemo = new FireworksDemo(ImageSurface);

            ImageSurface.Source = _fireworksDemo.Bitmap;
            CompositionTarget.Rendering += _fireworksDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _fireworksDemo.Fire(e.GetPosition(ImageSurface).X.ToSimUnits(), e.GetPosition(ImageSurface).Y.ToSimUnits());
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (Drag.IsChecked == true)
            {
                _fireworksDemo.Type = FireworksDemo.PhysicsType.Water;
            }
            else if (Wind.IsChecked == true)
            {
                _fireworksDemo.Type = FireworksDemo.PhysicsType.Wind;
            }
        }
    }
}