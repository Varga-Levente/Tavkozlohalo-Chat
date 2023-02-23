using System;
using System.Net.Sockets;

namespace Chat.Net
{
    class Server
    {
        TcpClient _client;
        private String _host = "localhost";
        private int _port = 1298;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer()
        {
            if(!_client.Connected)
            {
                _client.Connect(_host.ToString(), (int)_port); ;
            }
        }
    }
}
