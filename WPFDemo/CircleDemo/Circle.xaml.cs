using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.CircleDemo
{
    /// <summary>
    /// Circle.xaml 的交互逻辑
    /// </summary>
    public partial class Circle : Window
    {
        private readonly CircleDemo _circleDemo;

        public Circle()
        {
            InitializeComponent();
            //_circleDemo = new CircleDemo(ImageSurface);

            //ImageSurface.Source = _circleDemo.Bitmap;
            //CompositionTarget.Rendering += _circleDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CircleDemo.Fire();
        }
    }
}