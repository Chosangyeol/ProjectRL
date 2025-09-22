using System;
using System.Collections.Generic;

namespace Player
{
	// TODO!
	[Serializable]
	public class PlayerComponentBuff
	{
		private PlayerModel playerModel;
		private List<BuffInfo> ListBuff;

		public PlayerComponentBuff(PlayerModel model)
		{
			playerModel = model;
			return ;
		}
	}
}
