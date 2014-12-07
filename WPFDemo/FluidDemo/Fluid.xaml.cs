using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo
{
    /// <summary>
    /// Fluid.xaml 的交互逻辑
    /// </summary>
    public partial class Fluid : Window
    {
        private FluidDemo.FluidDemo fluidDemo;

        public Fluid()
        {
            InitializeComponent();
            fluidDemo = new FluidDemo.FluidDemo(imageSurface);

            imageSurface.Source = fluidDemo.bitmap;
            CompositionTarget.Rendering += fluidDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            fluidDemo.Fire();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CompositionTarget.Rendering -= fluidDemo.Update;
        }
    }
}