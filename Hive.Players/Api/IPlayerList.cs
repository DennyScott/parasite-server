using System;
using System.Collections.Generic;
using Hive.Players.Components;

namespace Hive.Players.Api
{
    public interface IPlayerList
    {
        Dictionary<PlayerNames, IPlayer> Players { get; }
        IPlayer AddPlayer(UserConnection client);
        void RemovePlayer(IPlayer player);
        IPlayer GetPlayer(PlayerNames name);
    }
}
