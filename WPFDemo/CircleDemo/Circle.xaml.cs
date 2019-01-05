namespace WPFDemo.CircleDemo
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Circle.xaml 的交互逻辑
    /// </summary>
    public partial class Circle : Window
    {
        private readonly CircleDemo circleDemo;

        public Circle()
        {
            this.InitializeComponent();
            this.circleDemo = new CircleDemo(this.ImageSurface);

            this.ImageSurface.Source = this.circleDemo.Bitmap;
            CompositionTarget.Rendering += this.circleDemo.Update;
        }

        private void ImageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.circleDemo.Fire();
        }
    }
}