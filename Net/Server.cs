using Chat.Net.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Net
{
    class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;


        public event Action connectedEvent;
        public event Action msgRecievedEvent;
        public event Action userDisconnectEvent;
        public event Action IncomingFile;


        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {   
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect(ConfigurationManager.AppSettings["Chat_Server"], Int32.Parse(ConfigurationManager.AppSettings["Chat_Server_Port"]));
                    if (!_client.Connected)
                    {
                        MessageBox.Show("Could not connect to server", "Server Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    PacketReader = new PacketReader(_client.GetStream());

                    if (!string.IsNullOrEmpty(username))
                    {
                        var connectPacket = new PacketBuilder();
                        connectPacket.WriteOpCode(0);
                        connectPacket.WriteMessage(username);
                        _client.Client.Send(connectPacket.GetPacketBytes());
                    }
                    ReadPackets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Server Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool IsConnected()
        {
            return _client.Connected;
        }

        private void ReadPackets()
        {
            Task.Run(() => 
            { 
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgRecievedEvent?.Invoke();
                            break;
                        case 55:
                            msgRecievedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;
                        case 21:
                            IncomingFile?.Invoke();
                            break;
                        default:
                            Console.WriteLine("Default");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(String message) 
        {
            var messagePacket = new PacketBuilder();
            if (message[0]== char.Parse("/"))
            {
                messagePacket.WriteOpCode(55);
            }
            else
            {
                messagePacket.WriteOpCode(5);
            }
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void SendFileRequest(String filename, String toUser, String downloadurl)
        {
            var packet = new PacketBuilder();
            packet.WriteOpCode(20);
            packet.WriteMessage($"{toUser}|{filename}|{downloadurl}");
            _client.Client.Send(packet.GetPacketBytes());
        }
    }
}
