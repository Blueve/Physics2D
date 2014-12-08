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
        private FireworksDemo.FireworksDemo fireworksDemo;

        public Fireworks()
        {
            InitializeComponent();
            fireworksDemo = new FireworksDemo.FireworksDemo(imageSurface);

            imageSurface.Source = fireworksDemo.bitmap;
            CompositionTarget.Rendering += fireworksDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            fireworksDemo.Fire((float)e.GetPosition(imageSurface).X / 50, (float)e.GetPosition(imageSurface).Y / 50);
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (Drag.IsChecked == true)
            {
                fireworksDemo.Type = FireworksDemo.FireworksDemo.WATER_G;
            }
            else if (Wind.IsChecked == true)
            {
                fireworksDemo.Type = FireworksDemo.FireworksDemo.WIND_G;
            }
        }
    }
}