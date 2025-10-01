using Player.Skill;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerComponentSkill
	{
		private PlayerModel playerModel;

		private APlayerSkill[] skills;
		private APlayerSkill[] activeSkill;

		public PlayerComponentSkill(PlayerModel model, APlayerSkillDataSO[] skillDatas)
		{
			playerModel = model;
			skills = new APlayerSkill[skillDatas.Length];
			activeSkill = new APlayerSkill[4];
			for (int i = 0; i < skills.Length; i++)
			{
				skills[i] = skillDatas[i].CreateSkill();
			}
			return ;
		}

		public bool UpdateSkill(float delta)
		{
			bool result = false;

			for (int i = 0; i < activeSkill.Length; i++)
			{
				activeSkill[i]?.UpdateSkill(delta);
			}
			return (result);
		}

		public bool UseSkill(short index)
		{
			if (index > activeSkill.Length || index < 0)
				return (false);
			if (activeSkill[index] == null)
				throw new Exception($"unknown skill {index}");
			return (activeSkill[index].UseSkill(playerModel));
		}

		public bool SetSkill(short targetIndex, short skillIndex)
		{
			if (targetIndex > activeSkill.Length || targetIndex < 0 || skillIndex < 0 || skillIndex > skills.Length)
				return (false);
			if (activeSkill[targetIndex] != null)
				return (false);
			if (skills[skillIndex] == null)
				return (false);
			activeSkill[targetIndex] = skills[skillIndex];
			return (true);
		}

		public APlayerSkill[] GetActiveSkill()
		{
			return (activeSkill);
		}
	}
}
