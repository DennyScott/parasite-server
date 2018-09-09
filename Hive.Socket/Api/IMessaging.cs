using System;
using Hive.Contracts;
using Hive.Players.Api;

namespace Hive.Socket.Api
{
    public interface IMessaging
    {
        void Broadcast(Beat socketMessage);
        void Broadcast(Channel command, string message, IPlayer player);
        void SendToPlayer(Channel command, string message, IPlayer player);
        void SendToPlayer(Beat socketMessage, IPlayer player);
        void SendToClients(Beat socketMessage);
        void SendToClients(string command, string message, IPlayer player);
    }
}
