using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo
{
    /// <summary>
    /// Circle.xaml 的交互逻辑
    /// </summary>
    public partial class Circle : Window
    {
        private CircleDemo.CircleDemo circleDemo;

        public Circle()
        {
            InitializeComponent();
            circleDemo = new CircleDemo.CircleDemo(imageSurface);

            imageSurface.Source = circleDemo.bitmap;
            CompositionTarget.Rendering += circleDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            circleDemo.Fire();
        }
    }
}