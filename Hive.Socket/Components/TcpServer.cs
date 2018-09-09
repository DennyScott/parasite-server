using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using Hive.Players.Components;
using Newtonsoft.Json;
using Hive.Contracts;
using Hive.Socket.Api;
using Hive.Players.Api;
using ProtoBuf;

namespace Hive.Socket
{
    public class TcpServer
    {
        private static TcpListener listener { get; set; }
        private static bool accept { get; set; } = false;
		private readonly IConnect _connection;
		private readonly IMessaging _messaging;
		private readonly IPlayerList _playerList;

        public TcpServer(IConnect connection, IMessaging messaging, IPlayerList playerList)
        {
            _connection = connection;
            _messaging = messaging;
            _playerList = playerList;

            StartServer(5678);
            Listen();
        }

        public void StartServer(int port)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(address, port);

            listener.Start();
            accept = true;

            Console.WriteLine($"Server started. Listeneing to TCP Clients at 127.0.0.1:{port}");
        }

        public void StopServer()
        {
            accept = false;
            Console.WriteLine("Server shutting down");
            listener.Stop();
            _playerList.Dispose();
        }

        public void Listen()
        {
            if(listener != null && accept)
            {
                //Continue Listening
                while(true)
                {
                    Console.WriteLine("Waiting for the client...");
                    var clientTask = listener.AcceptTcpClientAsync(); //Get the client

                    if(clientTask.Result != null)
                    {
                        Console.WriteLine("Start Connection process");
                        var client = new UserConnection(clientTask.Result, OnLineReceived);
                    }
                }
            }
        }

        private void OnLineReceived(UserConnection sender, Stream data)
        {
            Console.WriteLine("Line Recieved");

            var socketMessage = Serializer.DeserializeWithLengthPrefix<Beat>(data, PrefixStyle.Base128);

            Console.WriteLine($"MessageType: {socketMessage.MessageType}");

            switch(socketMessage.MessageType)
            {
                case MessageType.Connect:
                    _connection.ConnectUser("d", sender);
                    break;
                case MessageType.Broadcast:
                    //_messaging.Broadcast(socketMessage);
                    break;
                case MessageType.Disconnect:
                    _connection.DisconnectUser(_playerList.GetPlayer(PlayerNames.PlayerOne));
                    break;
                default:
                    _messaging.SendToClients(socketMessage);
                    break;
            }
        }

    }
}
