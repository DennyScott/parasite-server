using System;
using Hive.Players.Api;

namespace Hive.Players.Components
{
    public class Player : IPlayer
    {
        public UserConnection Client { get; set; }
        public PlayerNames Name { get; set; }
    }
}
