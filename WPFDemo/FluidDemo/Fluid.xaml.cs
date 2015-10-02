using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.FluidDemo
{
    /// <summary>
    /// Fluid.xaml 的交互逻辑
    /// </summary>
    public partial class Fluid
    {
        private readonly FluidDemo _fluidDemo;

        public Fluid()
        {
            InitializeComponent();
            _fluidDemo = new FluidDemo(ImageSurface);

            ImageSurface.Source = _fluidDemo.Bitmap;
            CompositionTarget.Rendering += _fluidDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _fluidDemo.Fire();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CompositionTarget.Rendering -= _fluidDemo.Update;
        }
    }
}