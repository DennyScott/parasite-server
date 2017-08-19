using System;
using Hive.Players.Components;

namespace Hive.Players.Api
{
    public interface IPlayer
    {
        UserConnection Client { get; set; }
        PlayerNames Name { get; set; }
    }
}
