using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using MySql.Data;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Drag_Window(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginBTN(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Username.Text))
            {
                Username.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#c94238");
            }

            if (string.IsNullOrEmpty(Password.Password))
            {
                Password.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#c94238");
            }

            if(!string.IsNullOrEmpty(Password.Password) && !string.IsNullOrEmpty(Username.Text))
            {

            }
        }
    }
}
