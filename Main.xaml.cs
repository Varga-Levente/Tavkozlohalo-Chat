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

namespace Chat
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private static String username;
        private static String server_ip;
        public Window1(String Usrname, String SRVIP)
        {
            username = Usrname;
            server_ip = SRVIP;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void Drag_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            username_txt.Text = "@"+ username;
            mymessage.Focus();
        }
    }
}
