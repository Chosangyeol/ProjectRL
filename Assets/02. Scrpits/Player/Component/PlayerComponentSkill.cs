using System;

namespace Player.Component
{
	// TODO!
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
