using System;
using System.Net.Sockets;
using System.Text;
using Hive.Contracts;
using ProtoBuf;

namespace ProjectDemo
{
    class Program
    {
        private static int ReadBufferSize = 255;
        private static readonly byte[] _readBuffer = new byte[ReadBufferSize];
        private static TcpClient _client;
        public static string Res = string.Empty;
        public static string StrMessage = string.Empty;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            _client = new TcpClient("127.0.0.1", 5678);
            _client.GetStream().BeginRead(_readBuffer, 0, ReadBufferSize, DoRead, null);
        }

        static void DoRead(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("try do read");
                // Finish asynchronous read into readBuffer and return number of bytes read.
                var bytesRead = _client.GetStream().EndRead(ar);
                if (bytesRead < 1)
                {
                    Console.WriteLine("Server closed");
                    // if no bytes were read server has close.  
                    Res = "Disconnected";
                    return;
                }
                Console.WriteLine("String message");
                // Convert the byte array the message was saved into, minus two for the
                // Chr(13) and Chr(10)
                var strMessage = Serializer.Deserialize<Beat>(_client.GetStream());
                Console.WriteLine(strMessage.MessageType);
                // Start a new asynchronous read into readBuffer.
                _client.GetStream().BeginRead(_readBuffer, 0, ReadBufferSize, DoRead, null);
            }
            catch
            {
                Res = "Disconnected";
            }
        }
        
    }
}