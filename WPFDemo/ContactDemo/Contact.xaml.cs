namespace WPFDemo.ContactDemo
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Contact.xaml 的交互逻辑
    /// </summary>
    public partial class Contact : Window
    {
        private readonly ContactDemo contactDemo;

        public Contact()
        {
            this.InitializeComponent();

            this.contactDemo = new ContactDemo(this.ImageSurface);
            this.ImageSurface.Source = this.contactDemo.Bitmap;
            CompositionTarget.Rendering += this.contactDemo.Update;
        }

        private void ImageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.contactDemo.Fire();
        }
    }
}
