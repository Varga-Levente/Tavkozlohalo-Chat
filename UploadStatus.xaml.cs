using System.Windows;

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

        // Main window constructor
        public UploadStatus()
        {
            InitializeComponent();
        }

        // Set the progress bar value and text
        public void SetProgress(double progress)
        {
            Dispatcher.Invoke(() => {
                uploadprogress.Value = progress;
                uploadtext.Text = "Upload: "+progress.ToString();
            });
        }
    }
}
