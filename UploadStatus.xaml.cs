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
    /// Interaction logic for UploadStatus.xaml
    /// </summary>
    /// 
    
    public partial class UploadStatus : Window
    {
        // Close the window custom button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public UploadStatus()
        {
            InitializeComponent();
        }

        public void SetProgress(double progress)
        {
            Dispatcher.Invoke(() => {
                uploadprogress.Value = progress;
                uploadtext.Text = progress.ToString();
            });
        }
    }
}
