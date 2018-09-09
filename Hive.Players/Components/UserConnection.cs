using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Hive.Contracts;
using Newtonsoft.Json;
using ProtoBuf;

namespace Hive.Players.Components
{
    public class UserConnection
    {
        const int READ_BUFFER_SIZE = 1024;
        private TcpClient _client;
        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];

        private Action<UserConnection, Stream> OnLineReceived;

        public UserConnection(TcpClient clientTask, Action<UserConnection, Stream> onEvent)
        {
            _client = clientTask;
			OnLineReceived += onEvent;
            _client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(StreamReciever), null);
            Console.WriteLine("User Connected");
        }

        public void SendData(Beat data)
        {
            lock (_client.GetStream())
            {
                Serializer.Serialize(_client.GetStream(), data);
            }
        }

        public void CloseConnection() {
            _client.Close();
            _client.Dispose();
        }

        public void StreamReciever(IAsyncResult ar)
        {
            int bytesRead;
            string message;

            try
            {
                lock(_client.GetStream())
                {
                    // Finish asynch read into readBuffer and get num of bytes read
                    bytesRead = _client.GetStream().EndRead(ar);
                }
                
                Console.WriteLine(bytesRead);

                //var m = Encoding.ASCII.GetString(readBuffer, 0, bytesRead - 1);
                // Convert the byte array the message was saved into, minus Chr(13)
                OnLineReceived(this, _client.GetStream());

                //Ensure that no other threads try to use this thread at the same time
                lock(_client.GetStream())
                {
                    // Starts a new asynch read into readBuffer
                    _client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(StreamReciever), null);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
