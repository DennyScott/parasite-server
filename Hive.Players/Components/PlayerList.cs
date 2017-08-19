using System;
using System.Collections.Generic;
using Autofac;
using Hive.Players.Api;

namespace Hive.Players.Components
{
    public class PlayerList : IPlayerList
    {
        private readonly List<PlayerNames> _availableNames;
        private const int MaxPlayerSize = 2;
        private readonly IComponentContext _container;

        public PlayerList(IComponentContext container)
        {
            _container = container;
            Players = new Dictionary<PlayerNames, IPlayer>();
            _availableNames = new List<PlayerNames>
            {
                PlayerNames.PlayerOne,
                PlayerNames.PlayerTwo
            };
        }

        public Dictionary<PlayerNames, IPlayer> Players { get; }

        public IPlayer AddPlayer(UserConnection client)
        {
            PlayerNames playerName;

            if (!TryGetAvailablePlayer(out playerName)) return null;

            var player = _container.Resolve<IPlayer>();

            player.Client = client;
            player.Name = playerName;

            if (!Players.ContainsKey(playerName))
                Players.Add(playerName, player);

            Console.WriteLine("Player Added!");

            return player;
        }

        public IPlayer GetPlayer(PlayerNames name)
        {
            return !Players.ContainsKey(name) ? null : Players[name];
        }

        public void RemovePlayer(IPlayer player)
        {
            _availableNames.Add(player.Name);
            Players.Remove(player.Name);
        }

		private bool TryGetAvailablePlayer(out PlayerNames name)
		{
			name = PlayerNames.Unassigned;
            Console.WriteLine(Players.Count);
			if (Players.Count >= MaxPlayerSize) return false;

            Console.WriteLine(_availableNames.Count);

			name = _availableNames.PopAt(0);
			return true;
		}
    }
}
