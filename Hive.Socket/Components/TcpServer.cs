using System;
using System.Net.Sockets;
using System.Net;
using Hive.Players.Components;
using Newtonsoft.Json;
using Hive.Contracts;
using Hive.Socket.Api;
using Hive.Players.Api;

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
                        var client = new UserConnection(clientTask.Result);
                        client.LineRecieved += OnLineReceived;
                    }
                }
            }
        }

        private void OnLineReceived(UserConnection sender, string data)
        {
            Console.WriteLine("Line Recieved");

            var dataArray = data.Split((char)13);
            Console.WriteLine(dataArray[0]);

            var socketMessage = JsonConvert.DeserializeObject<SocketMessage>(data);

            Console.WriteLine(socketMessage.Command);

            switch(socketMessage.Command)
            {
                case "CONNECT":
                    _connection.ConnectUser(socketMessage.Player, sender);
                    break;
                case "BROADCAST":
                    _messaging.Broadcast(socketMessage);
                    break;
                case "DISCONNECT":
                    _connection.DisconnectUser(_playerList.GetPlayer(PlayerNamesUtil.StringToEnum(socketMessage.Player)));
                    break;
                default:
                    _messaging.SendToClients(socketMessage);
                    break;
            }
        }

    }
}
