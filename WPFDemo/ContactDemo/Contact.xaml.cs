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

using WPFDemo.ContactDemo;

namespace WPFDemo
{
    /// <summary>
    /// Contact.xaml 的交互逻辑
    /// </summary>
    public partial class Contact : Window
    {
        private ContactDemo.ContactDemo contactDemo;


        public Contact()
        {
            InitializeComponent();

            contactDemo = new ContactDemo.ContactDemo(imageSurface);
            imageSurface.Source = contactDemo.bitmap;
            CompositionTarget.Rendering += contactDemo.Update;
        }

        private void imageSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            contactDemo.Fire();
        }
    }
}
