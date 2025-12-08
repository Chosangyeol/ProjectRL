using JetBrains.Annotations;
using Player.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Player.Component
{
	[Serializable]
	public class PlayerComponentSkill
	{
		protected PlayerModel playerModel;

		protected APlayerSkill[] skills;
		protected APlayerSkill[] activeSkills;

		public PlayerComponentSkill(PlayerModel model, APlayerSkillDataSO[] skillDatas)
		{
			playerModel = model;
			skills = new APlayerSkill[skillDatas.Length];
			for (int i = 0; i < skills.Length; i++)
			{
				skills[i] = skillDatas[i]?.CreateSkill();
			}
			SetUpActiveSkill();
			return ;
		}

		protected virtual void SetUpActiveSkill()
		{
			activeSkills = new APlayerSkill[4];

			for (short i = 0; i < activeSkills.Length; i++)
			{
				int idx = PlayerPrefs.GetInt($"Skill{i}", -1);

				if (idx != -1)
				{
					SetSkill(i, (short)idx);
				}
				else
				{
					SetSkill(i, i);
				}
			}
			return ;
		}

		public virtual bool UpdateSkill(float delta)
		{
			bool result = false;

			for (int i = 0; i < activeSkills.Length; i++)
			{
				activeSkills[i]?.UpdateSkill(delta);
			}
			return (result);
		}

		public virtual bool UseSkill(short index, KeyCode skillKey)
		{
			if (index > activeSkills.Length || index < 0)
				return (false);
			if (activeSkills[index] == null)
				throw new Exception($"unknown skill {index}");
			return (activeSkills[index].UseSkill(playerModel, skillKey));
		}

		public bool SetSkill(short targetIndex, string skillName)
		{
			if (targetIndex > activeSkills.Length || targetIndex < 0)
				return (false);
			try
			{
				Type type;
				APlayerSkill skill;

				if (activeSkills[targetIndex] != null)
					return (false);
				type = PlayerSkill.skillTypes[skillName];
				skill = skills.FirstOrDefault(s => s.GetType() == type);
				if (skill == null)
					return (false);
				SetSkill(targetIndex, skill);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				return (false);
			}
			return (true);
		}

		public bool SetSkill(short targetIndex, short skillIndex)
		{
			if (targetIndex > activeSkills.Length || targetIndex < 0 || skillIndex < 0 || skillIndex > skills.Length)
				return (false);
			if (activeSkills[targetIndex] != null)
				return (false);
			if (skills[skillIndex] == null)
				return (false);
			SetSkill(targetIndex, skills[skillIndex]);
			return (true);
		}

		protected virtual void SetSkill(short targetIndex, APlayerSkill skill)
		{
			int idx = Array.FindIndex(activeSkills, a => a.GetType() == skill.GetType());

			if (targetIndex == idx)
			{
				activeSkills[targetIndex].IsSelected = false;
				activeSkills[targetIndex] = null;
			}
			else
			{
				activeSkills[targetIndex].IsSelected = false;
				activeSkills[targetIndex] = skill;
				activeSkills[targetIndex].IsSelected = true;
			}
			if (idx != -1)
			{
				activeSkills[idx] = null;
			}
			return ;
		}

		public APlayerSkill[] GetActiveSkill()
		{
			return (activeSkills);
		}
	}
}
