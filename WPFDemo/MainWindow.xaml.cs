using System.Windows;

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
            Fireworks fwW = new Fireworks();
            fwW.Show();
        }

        private void springDemo_Click(object sender, RoutedEventArgs e)
        {
        }

        private void circlrDemo_Click(object sender, RoutedEventArgs e)
        {
            Circle cW = new Circle();
            cW.Show();
        }

        private void elasticDemo_Click(object sender, RoutedEventArgs e)
        {
            Elastic eW = new Elastic();
            eW.Show();
        }

        private void fluidDemo_Click(object sender, RoutedEventArgs e)
        {
            Fluid fW = new Fluid();
            fW.Show();
        }
    }
}