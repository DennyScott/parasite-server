using System;
using System.Net.Sockets;
using System.Net;
using Hive.Players.Components;
using Newtonsoft.Json;
using Hive.Contracts;

namespace Hive.Core
{
    public class TcpServer
    {
        private static TcpListener listener { get; set; }
        private static bool accept { get; set; } = false;

        public static void StartServer(int port)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(address, port);

            listener.Start();
            accept = true;

            Console.WriteLine($"Server started. Listeneing to TCP Clients at 127.0.0.1:{port}");
        }

        public static void Listen()
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

                        Console.WriteLine("New User Connection starting");

                        //Console.WriteLine("Client Connected. Waiting for data.");
                        //var client = clientTask.Result;
                        //string message = "";

                        //while (message != null && !message.StartsWith("quit"))
                        //{
                        //    byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                        //    client.GetStream().Write(data, 0, data.Length);

                        //    byte[] buffer = new byte[1024];
                        //    client.GetStream().Read(buffer, 0, buffer.Length);

                        //    message = Encoding.ASCII.GetString(buffer);
                        //    Console.WriteLine(message);
                        //}

                        //Console.WriteLine("Closing connection");
                        //client.GetStream().Dispose();
                    }
                }
            }
        }

        private static void OnLineReceived(UserConnection sender, string data)
        {
            Console.WriteLine("Line Recieved");

            var dataArray = data.Split((char)13);
            Console.WriteLine(dataArray[0]);

            var socketMessage = JsonConvert.DeserializeObject<SocketMessage>(data);

            Console.WriteLine(socketMessage.Command);

            switch(socketMessage.Command)
            {
                case "CONNECT":
                    Console.WriteLine("Connect Command");
                    break;
                default:
                    Console.WriteLine("Default");
                    break;
            }
        }

    }
}
