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

using Physics2D;

namespace WPFDemo.RobDemo
{
    /// <summary>
    /// Rob.xaml 的交互逻辑
    /// </summary>
    public partial class Rob : Window
    {
        private readonly RobDemo _robDemo;

        public Rob()
        {
            InitializeComponent();

            _robDemo = new RobDemo(ImageSurface);
            ImageSurface.Source = _robDemo.Bitmap;
            CompositionTarget.Rendering += _robDemo.Update;
        }

        private void ImageSurface_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _robDemo.Down(e.GetPosition(ImageSurface).X.ToSimUnits(), e.GetPosition(ImageSurface).Y.ToSimUnits());
        }

        private void ImageSurface_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _robDemo.Up();
        }

        private void ImageSurface_OnMouseMove(object sender, MouseEventArgs e)
        {
            _robDemo.Move(e.GetPosition(ImageSurface).X.ToSimUnits(), e.GetPosition(ImageSurface).Y.ToSimUnits());
        }
    }
}
