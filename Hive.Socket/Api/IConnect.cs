using System;
using Hive.Players.Api;
using Hive.Players.Components;

namespace Hive.Socket.Api
{
    public interface IConnect
    {
        void ConnectUser(string userName, UserConnection sender);
        void DisconnectUser(IPlayer player);
    }
}
