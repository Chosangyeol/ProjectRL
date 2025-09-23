using Info;
using System;
using System.Collections.Generic;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerComponentBuff
	{
		private PlayerModel playerModel;
		private List<SInfoBuff> ListBuff;

		public PlayerComponentBuff(PlayerModel model)
		{
			playerModel = model;
			return ;
		}
	}
}
