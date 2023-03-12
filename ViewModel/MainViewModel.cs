using Chat.Core;
using Chat.Model;
using Chat.Net;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;

namespace Chat.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<String> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public RelayCommand SendFileRequest { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string toUser { get; set; }
        public string filename { get; set; }
        public string downloadurl { get; set; }
        public Boolean IsConnected { get; set; }
        private Server _server;

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();

            _server = new Server();

            _server.connectedEvent += UserConnected;
            _server.msgRecievedEvent += MessageRecieved;
            _server.userDisconnectEvent += RemoteUser;
            _server.IncomingFile += IncomingFile;

            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
            SendFileRequest = new RelayCommand(
                o => _server.SendFileRequest(toUser, filename, downloadurl),
                o => !string.IsNullOrEmpty(toUser) && !string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(downloadurl)
            );


            //If connection is established set IsConnected to true (ONLY FOR TESTING)
            IsConnected = _server.IsConnected();
        }

        private async void IncomingFile()
        {
            var msg = _server.PacketReader.ReadMessage();
            var msgparts = msg.Split("|");
            var fromUser = msgparts[0];
            var filename = Path.GetFileName(msgparts[1]).ToUTF8();
            var downloadurl = msgparts[2];

            var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", filename);

            var result = MessageBox.Show($"Do you want to download {filename}?", "Download file", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Dispatcher.Invoke(() => Messages.Add("[FILE] - File download started in the background"));
                using (var client = new WebClient())
                {
                    try
                    {
                        await client.DownloadFileTaskAsync(downloadurl, downloadPath);
                        Application.Current.Dispatcher.Invoke(() => Messages.Add("[FILE] - Download completed"));
                        MessageBox.Show($"File downloaded to {downloadPath}", "Download complete", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() => Messages.Add("[FILE ERROR] - Error while downloading"));
                        MessageBox.Show($"Error downloading file: {ex.Message}", "Download error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // This method is called when a user disconnects from the server
        private void RemoteUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        // This method is called when a message is recieved from the server
        private void MessageRecieved()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        // This method is called when a user connects to the server
        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }

        }
    }
}
