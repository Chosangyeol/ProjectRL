using Player.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerComponentSkill
	{
		protected PlayerModel playerModel;

		protected APlayerSkill[] skills;
		protected APlayerSkill[] activeSkill;

		public PlayerComponentSkill(PlayerModel model, APlayerSkillDataSO[] skillDatas)
		{
			playerModel = model;
			skills = new APlayerSkill[skillDatas.Length];
			activeSkill = new APlayerSkill[4];
			for (int i = 0; i < skills.Length; i++)
			{
				skills[i] = skillDatas[i]?.CreateSkill();
				if (skills[i] != null)
				{
					activeSkill[i] = skills[i];
				}
			}
			return ;
		}

		public virtual bool UpdateSkill(float delta)
		{
			bool result = false;

			for (int i = 0; i < activeSkill.Length; i++)
			{
				activeSkill[i]?.UpdateSkill(delta);
			}
			return (result);
		}

		public virtual bool UseSkill(short index, KeyCode skillKey)
		{
			if (index > activeSkill.Length || index < 0)
				return (false);
			if (activeSkill[index] == null)
				throw new Exception($"unknown skill {index}");
			return (activeSkill[index].UseSkill(playerModel, skillKey));
		}

		public virtual bool SetSkill(short targetIndex, string skillName)
		{
			if (targetIndex > activeSkill.Length || targetIndex < 0)
				return (false);
			try
			{
				Type type;
				APlayerSkill skill;

				if (activeSkill[targetIndex] != null)
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
			if (targetIndex > activeSkill.Length || targetIndex < 0 || skillIndex < 0 || skillIndex > skills.Length)
				return (false);
			if (activeSkill[targetIndex] != null)
				return (false);
			if (skills[skillIndex] == null)
				return (false);
			SetSkill(targetIndex, skills[skillIndex]);
			return (true);
		}

		protected virtual void SetSkill(short targetIndex, APlayerSkill skill)
		{
			int idx = Array.FindIndex(activeSkill, a => a.GetType() == skill.GetType());
			activeSkill[targetIndex] = skill;
			if (idx != -1)
				activeSkill[idx] = null;
			return ;
		}

		public APlayerSkill[] GetActiveSkill()
		{
			return (activeSkill);
		}
	}
}
