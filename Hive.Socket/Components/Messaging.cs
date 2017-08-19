using System;
using Hive.Contracts;
using Hive.Players.Api;
using Hive.Players.Components;
using Hive.Socket.Api;

namespace Hive.Socket.Components
{
    public class Messaging : IMessaging
    {
        IPlayerList _playerList;

        public Messaging(IPlayerList playerList)
        {
            _playerList = playerList;
        }

        public void Broadcast(SocketMessage socketMessage)
        {
            foreach(var player in _playerList.Players)
            {
                player.Value.Client.SendData(socketMessage);
            }
        }

        public void Broadcast(string command, string message, IPlayer player)
        {
            Broadcast(new SocketMessage()
            {
                Command = command,
                Payload = message,
                Player = PlayerNamesUtil.EnumToString(player.Name)
            });
        }

        public void SendToPlayer(string command, string message, IPlayer player)
        {
            player.Client.SendData(new SocketMessage()
            {
                Command = command,
                Payload = message,
                Player = PlayerNamesUtil.EnumToString(player.Name)
            });
        }

		public void SendToPlayer(SocketMessage message, IPlayer player)
		{
			player.Client.SendData(message);
		}

		public void SendToClients(SocketMessage socketMessage)
		{
			Console.WriteLine("Start the send.");
			Console.WriteLine(_playerList.Players.Count);
			foreach (var player in _playerList.Players)
			{
				if (PlayerNamesUtil.EnumToString(player.Value.Name) != socketMessage.Player)
				{
					Console.WriteLine("Send to clients");
					player.Value.Client.SendData(socketMessage);
				}

			}
		}

		public void SendToClients(string command, string message, IPlayer player)
		{
			var socketMessage = new SocketMessage
			{
				Payload = message,
				Player = PlayerNamesUtil.EnumToString(player.Name),
				Command = command,
			};
			SendToClients(socketMessage);
		}
    }
}
