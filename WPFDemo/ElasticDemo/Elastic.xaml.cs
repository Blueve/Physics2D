using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo
{
    /// <summary>
    /// ElasticDemo.xaml 的交互逻辑
    /// </summary>
    public partial class Elastic : Window
    {
        private ElasticDemo.ElasticDemo elasticDemo;

        public Elastic()
        {
            InitializeComponent();

            elasticDemo = new ElasticDemo.ElasticDemo(imageSurface);

            imageSurface.Source = elasticDemo.bitmap;
            CompositionTarget.Rendering += elasticDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            elasticDemo.Fire();
        }
    }
}