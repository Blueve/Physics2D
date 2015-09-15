using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.ContactDemo
{
    /// <summary>
    /// Contact.xaml 的交互逻辑
    /// </summary>
    public partial class Contact : Window
    {
        private readonly ContactDemo _contactDemo;


        public Contact()
        {
            InitializeComponent();

            _contactDemo = new WPFDemo.ContactDemo.ContactDemo(ImageSurface);
            ImageSurface.Source = _contactDemo.Bitmap;
            CompositionTarget.Rendering += _contactDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _contactDemo.Fire();
        }
    }
}
