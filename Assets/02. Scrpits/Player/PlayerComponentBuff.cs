using System;
using System.Collections.Generic;

namespace Player
{
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
