namespace Hive.Players.Components
{
	public enum PlayerNames
	{
		PlayerOne = 0,
		PlayerTwo = 1,
        Unassigned = 2
	}

	public static class PlayerNamesUtil
	{
		public static string EnumToString(PlayerNames name)
		{
			switch (name)
			{
				case PlayerNames.PlayerOne:
					return "PlayerOne";
				case PlayerNames.PlayerTwo:
					return "PlayerTwo";
				default:
					return "Unassigned";
			}
		}

		public static PlayerNames StringToEnum(string name)
		{
			switch (name)
			{
				case "PlayerOne":
					return PlayerNames.PlayerOne;
				case "PlayerTwo":
					return PlayerNames.PlayerTwo;
				default:
					return PlayerNames.Unassigned;
			}
		}
	}
}
