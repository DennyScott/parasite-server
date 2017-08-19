using System;
using Hive.Contracts;
using Hive.Players.Api;

namespace Hive.Socket.Api
{
    public interface IMessaging
    {
        void Broadcast(SocketMessage socketMessage);
        void Broadcast(string command, string message, IPlayer player);
        void SendToPlayer(string command, string message, IPlayer player);
        void SendToPlayer(SocketMessage socketMessage, IPlayer player);
        void SendToClients(SocketMessage socketMessage);
        void SendToClients(string command, string message, IPlayer player);
    }
}
