using Player.Skill;
using System;
using UnityEngine;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerSkillSoldierComponent : PlayerComponentSkill
	{
		public PlayerSkillSoldierComponent(PlayerModel model, APlayerSkillDataSO[] skillDatas) : base(model, skillDatas)
		{

		}

		protected override void SetUpActiveSkill()
		{
			activeSkills = new APlayerSkill[2];

			for (int i = 0; i < activeSkills.Length; i++)
			{
				int idx = PlayerPrefs.GetInt($"Skill{i}", -1);

				if (idx != -1)
					activeSkills[i] = skills[idx];
			}
			return;
		}
	}
}
