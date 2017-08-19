using System;
namespace Hive.Contracts
{
    public class SocketMessage
    {
        public string Command { get; set; }
        public string Payload { get; set; }
        public string Player { get; set; }
    }
}
