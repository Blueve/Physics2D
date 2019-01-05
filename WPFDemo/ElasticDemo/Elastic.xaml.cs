namespace WPFDemo.ElasticDemo
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// ElasticDemo.xaml 的交互逻辑
    /// </summary>
    public partial class Elastic : Window
    {
        private ElasticDemo elasticDemo;

        public Elastic()
        {
            this.InitializeComponent();

            this.elasticDemo = new ElasticDemo(this.ImageSurface);

            this.ImageSurface.Source = this.elasticDemo.Bitmap;
            CompositionTarget.Rendering += this.elasticDemo.Update;
        }

        private void ImageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.elasticDemo.Fire();
        }
    }
}