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

        public void Broadcast(Beat socketMessage)
        {
            foreach(var player in _playerList.Players)
            {
                player.Value.Client.SendData(socketMessage);
            }
        }

        public void Broadcast(Channel channel, string message, IPlayer player)
        {
            Broadcast(new Beat()
            {
                MessageType = Contracts.MessageType.Broadcast,
                Channel = channel,
            });
        }

        public void SendToPlayer(Channel command, string message, IPlayer player)
        {
            player.Client.SendData(new Beat()
            {
	            MessageType = Contracts.MessageType.Send
            });
        }

		public void SendToPlayer(Beat message, IPlayer player)
		{
			player.Client.SendData(message);
		}

		public void SendToClients(Beat socketMessage)
		{
			Console.WriteLine("Start the send.");
			Console.WriteLine(_playerList.Players.Count);
			foreach (var player in _playerList.Players)
			{
//				if (PlayerNamesUtil.EnumToString(player.Value.Name) != socketMessage.Player)
//				{
					Console.WriteLine("Send to clients");
					player.Value.Client.SendData(socketMessage);
//				}

			}
		}

		public void SendToClients(string command, string message, IPlayer player)
		{
			var socketMessage = new Beat
			{
				MessageType = Contracts.MessageType.Send
			};
			SendToClients(socketMessage);
		}
    }
}
