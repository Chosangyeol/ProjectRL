using System;

namespace Player.Item
{
	[Serializable]
	public class Inventory
	{
		private readonly PlayerModel playerModel;

		public Inventory(PlayerModel playerModel)
		{
			this.playerModel = playerModel;
			return ;
		}


	}
}
