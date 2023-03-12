using Chat.Net;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // Main window constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        // Get the config from the app.config file
        public static string Config(String name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        // Function to hash the password with SHA512 and to lower the string to match the database hash
        static string ComputeSha512Hash(string s)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(s));
                foreach (byte b in hashValue)
                {
                    sb.Append($"{b:X2}");
                }
            }

            return sb.ToString().ToLower();
        }

        // Make window draggable on top bar
        private void Drag_Window(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Cutom close button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // On press enter simulate login button click
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginBTN(null, null);
            }
        }

        // Function to login and check if the user is valid with the API POST request
        private async void LoginBTN(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Username.Text))
            {
                Username.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#c94238");
                errormesage_back.Visibility = Visibility.Visible;
                errormesage_text.Visibility = Visibility.Visible;
                errormesage_text.Text = "Empty Username";
            }
            else
            {
                Username.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("White");
            }

            if (string.IsNullOrEmpty(Password.Password))
            {
                Password.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#c94238");
                errormesage_back.Visibility = Visibility.Visible;
                errormesage_text.Visibility = Visibility.Visible;
                errormesage_text.Text = "Empty Password";
            }
            else
            {
                Password.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("White");
            }

            if (!string.IsNullOrEmpty(Password.Password) && !string.IsNullOrEmpty(Username.Text))
            {
                var authenticateUser = new AuthenticateUser(
                        Config("API_Server_Protocol")+"://"+
                        Config("API_Server_IP")+":"+
                        Config("API_Server_Port")+"/"+
                        Config("API_Server_CheckUserRoute")
                );

                var result = await authenticateUser.AuthenticateAsync(Username.Text, ComputeSha512Hash(Password.Password));

                var resultsplit = result.Split(':');

                if (resultsplit[0] == "Error")
                {
                    errormesage_text.Text = resultsplit[1];
                    errormesage_back.Visibility = Visibility.Visible;
                    errormesage_text.Visibility = Visibility.Visible;
                }
                else
                {
                    errormesage_text.Text = "Login Success";
                    errormesage_text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00ff26"));
                    errormesage_back.Visibility = Visibility.Visible;
                    errormesage_text.Visibility = Visibility.Visible;
                    Window1 _chatwindow = new(Username.Text, resultsplit[1]);
                    _chatwindow.Show();
                    this.Close();
                }
            }
        }
    }
}
