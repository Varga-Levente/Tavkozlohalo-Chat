﻿using Chat.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Chat.Model;

namespace Chat
{
    public partial class Window1 : Window
    {
        private static String username;
        private static String server_ip;
        private MainViewModel _viewModel;
        private UserModel _selectedUser;
        public Window1(String Usrname, String SRVIP)
        {
            username = Usrname;
            server_ip = SRVIP;
            InitializeComponent();
            _viewModel = new MainViewModel();
            _viewModel.Username = Usrname;
            DataContext = _viewModel;
        }

        // Window onload event
        void OnLoad(object sender, RoutedEventArgs e)
        {
            username_txt.Text = "@" + username;
            mymessage.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ConnectToServerCommand.Execute(null);

            try
            {
                _viewModel.ConnectToServerCommand.Execute(null);
                if (!_viewModel.IsConnected)
                {
                    mymessage.Text = "";
                    mymessage.IsReadOnly = false;
                    statusindicator.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                }

            }
            catch (Exception ex)
            {
                mymessage.Text = "You are not connected to the chat server! Reload the APP.";
                mymessage.IsReadOnly = true;
                statusindicator.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        // Context menu click events
        private void MenuItem_SendFile_Click(object sender, RoutedEventArgs e)
        {
            if (userlist.SelectedItem != null)
            {
                MessageBox.Show("This feature is not implemented yet.", "Missing function {MenuItem_SendFile_Click}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_VoiceCall_Click(object sender, RoutedEventArgs e)
        {
            if (userlist.SelectedItem != null)
            {
                MessageBox.Show("This feature is not implemented yet.", "Missing function {MenuItem_VoiceCall_Click}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_PrivateMessage_Click(object sender, RoutedEventArgs e)
        {
            if (userlist.SelectedItem != null)
            {
                UserModel selectedUser = (UserModel)userlist.SelectedItem;
                mymessage.Text = "/p " + selectedUser.Username + " ";
                // Focus the textbox to allow the user to type
                mymessage.Focus();
                // Deselect the item
                userlist.SelectedItem = null;
            }
        }

        // Send message with enter key and clear the textbox
        private void EnterSend(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.SendMessageCommand.Execute(null);
                mymessage.Text = "";
            }
        }

        private void CrearMessage(object sender, RoutedEventArgs e)
        {
            _viewModel.SendMessageCommand.Execute(null);
            mymessage.Text = "";
        }

        // Close the window custom button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Minimize the window custom button
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        // Make the window draggable on the custom title bar
        private void Drag_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
