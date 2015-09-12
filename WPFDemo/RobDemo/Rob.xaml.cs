using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFDemo.RobDemo
{
    /// <summary>
    /// Rob.xaml 的交互逻辑
    /// </summary>
    public partial class Rob : Window
    {
        private RobDemo _robDemo;

        public Rob()
        {
            InitializeComponent();

            _robDemo = new RobDemo(ImageSurface);
            ImageSurface.Source = _robDemo.Bitmap;
            CompositionTarget.Rendering += _robDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _robDemo.Fire();
        }
    }
}
