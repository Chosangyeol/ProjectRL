using Player.Skill;
using System;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerSkillSoldierComponent : PlayerComponentSkill
	{
		public PlayerSkillSoldierComponent(PlayerModel model, APlayerSkillDataSO[] skillDatas) : base(model, skillDatas)
		{

		}
	}
}
