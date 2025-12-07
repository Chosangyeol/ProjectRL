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
		protected List<SPlayerSkillDataSet> skillDataSets;
		protected Dictionary<int, Tuple<int, int>> skillToSkillDataMap;

		public SPlayerSkillDataSet[] SkillDataSets { get => skillDataSets.ToArray(); }

		public PlayerComponentSkill(PlayerModel model, APlayerSkillDataSO[] skillDatas)
		{
			playerModel = model;
			skills = new APlayerSkill[skillDatas.Length];
			skillDataSets = new List<SPlayerSkillDataSet>();
			skillToSkillDataMap = new Dictionary<int, Tuple<int, int>>();
			for (int i = 0; i < skills.Length; i++)
			{
				skills[i] = skillDatas[i]?.CreateSkill();
			}
			SetUpActiveSkill();
			MakeSkillDataSet();
			return ;
		}

		protected virtual void MakeSkillDataSet()
		{
			for (int i = 0; i < skills.Length; i++)
			{
				if (skills[i] == null)
					continue ;
				SPlayerSkillData data = new SPlayerSkillData(i, skills[i].dataSO, skills[i].IsSelected);

				skillDataSets.Add(new SPlayerSkillDataSet(i, data));
				skillToSkillDataMap.Add(i, Tuple.Create(i, 0));
			}
			return ;
		}

		protected virtual void SetUpActiveSkill()
		{
			activeSkills = new APlayerSkill[4];

			for (int i = 0; i < activeSkills.Length; i++)
			{
				if (skills[i] != null)
				{
					activeSkills[i] = skills[i];
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
				WriteSkillData(activeSkills[targetIndex]);
				activeSkills[targetIndex] = skill;
				activeSkills[targetIndex].IsSelected = true;
			}
			if (idx != -1)
			{
				activeSkills[idx] = null;
			}
			WriteSkillData(skill);
			return ;
		}

		public void WriteSkillData(APlayerSkill skill)
		{
			int tmp = Array.FindIndex(skills, s => s.GetType() == skill.GetType());
			Tuple<int, int> idxTuple;
			SPlayerSkillDataSet dataSet;
			SPlayerSkillData[] datas;

			if (!skillToSkillDataMap.TryGetValue(tmp, out idxTuple))
				throw (new Exception("너가 이걸 보고있다면, 무언가 심각히 잘못되었다."));
			dataSet = skillDataSets[idxTuple.Item1];
			datas = dataSet.GetDatas();
			datas[idxTuple.Item2].SetActive(skill.IsSelected);
			skillDataSets[idxTuple.Item1] = new SPlayerSkillDataSet(dataSet.GetTargetIndex(), datas);
			return ;
		}

		public APlayerSkill[] GetActiveSkill()
		{
			return (activeSkills);
		}
	}
}
