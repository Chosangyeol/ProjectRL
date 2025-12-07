using Info;
using Player.Component;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerSummonableMono : PoolableMono
	{
		protected PlayerModel	player;

		public void SetPlayer(PlayerModel player)
		{
			this.player = player;
			return ;
		}
	}
}
