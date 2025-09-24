using Player.Skill;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerComponentSkill
	{
		private PlayerModel playerModel;

		private APlayerSkill[] skills;

		public PlayerComponentSkill(PlayerModel model)
		{
			playerModel = model;
			skills = new APlayerSkill[4];
			return ;
		}

		public bool UpdateSkill(float delta)
		{
			bool result = false;

			for (int i = 0; i < skills.Length; i++)
			{
				skills[i].UpdateSkill(delta);
			}
			return (result);
		}

		public bool UseSkill(short index)
		{
			if (index > skills.Length || index < 0)
				return (false);
			return (skills[index].UseSkill(playerModel));
		}
	}
}
