using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.ElasticDemo
{
    /// <summary>
    /// ElasticDemo.xaml 的交互逻辑
    /// </summary>
    public partial class Elastic : Window
    {
        private ElasticDemo _elasticDemo;

        public Elastic()
        {
            InitializeComponent();

            _elasticDemo = new ElasticDemo(ImageSurface);

            ImageSurface.Source = _elasticDemo.Bitmap;
            CompositionTarget.Rendering += _elasticDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _elasticDemo.Fire();
        }
    }
}