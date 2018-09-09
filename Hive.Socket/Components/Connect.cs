using System;
using Hive.Contracts;
using Hive.Players.Api;
using Hive.Players.Components;
using Hive.Socket.Api;

namespace Hive.Socket.Components
{
    public class Connect : IConnect
    {
        private readonly IPlayerList _playerList;
        private readonly IMessaging _messaging;

        public Connect(IPlayerList playerList, IMessaging messaging)
        {
            _messaging = messaging;
            _playerList = playerList;
        }

        public void ConnectUser(string userName, UserConnection sender)
        {
            Console.WriteLine("Trying to connect");

            var player = _playerList.AddPlayer(sender);

            if(player == null)
            {
                _messaging.SendToPlayer(Channel.Lobby, "Connection Failed", new Player() {Client = sender});
            }
            else
            {
                var message = userName + " has joined the game!";

                Console.WriteLine(message);
                _messaging.SendToPlayer(Channel.Chat, message, player);
                _messaging.SendToClients("CHAT", message, player);
            }
        }

        public void DisconnectUser(IPlayer player)
        {
            Console.WriteLine($"{player.Name} has left the game.");

            _messaging.SendToClients("DISCONNECT", $"Player {player.Name} has disconnected.", player);
            _playerList.RemovePlayer(player);
        }
    }
}
