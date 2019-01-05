namespace WPFDemo.FluidDemo
{
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Fluid.xaml 的交互逻辑
    /// </summary>
    public partial class Fluid
    {
        private readonly FluidDemo fluidDemo;

        public Fluid()
        {
            this.InitializeComponent();
            this.fluidDemo = new FluidDemo(this.ImageSurface);

            this.ImageSurface.Source = this.fluidDemo.Bitmap;
            CompositionTarget.Rendering += this.fluidDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.fluidDemo.Fire();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CompositionTarget.Rendering -= this.fluidDemo.Update;
        }
    }
}