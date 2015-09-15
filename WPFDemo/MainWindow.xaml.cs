using System.Windows;
using WPFDemo.ContactDemo;
using WPFDemo.ElasticDemo;
using WPFDemo.FireworksDemo;
using WPFDemo.FluidDemo;
using WPFDemo.RobDemo;

namespace WPFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void fireworksDemo_Click(object sender, RoutedEventArgs e)
        {
            var fwW = new Fireworks();
            fwW.Show();
        }

        private void circlrDemo_Click(object sender, RoutedEventArgs e)
        {
            var cW = new CircleDemo.Circle();
            cW.Show();
        }

        private void elasticDemo_Click(object sender, RoutedEventArgs e)
        {
            var eW = new Elastic();
            eW.Show();
        }

        private void fluidDemo_Click(object sender, RoutedEventArgs e)
        {
            var fW = new Fluid();
            fW.Show();
        }

        private void contactDemo_Click(object sender, RoutedEventArgs e)
        {
            var cW = new Contact();
            cW.Show();
        }

        private void RobDemo_Click(object sender, RoutedEventArgs e)
        {
            var rW = new Rob();
            rW.Show();
        }
    }
}