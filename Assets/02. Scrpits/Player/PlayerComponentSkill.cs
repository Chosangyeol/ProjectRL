using System;

namespace Player
{
	[Serializable]
	public class PlayerComponentSkill
	{
		private PlayerModel playerModel;

		public PlayerComponentSkill(PlayerModel model)
		{
			playerModel = model;
			return ;
		}
	}
}
