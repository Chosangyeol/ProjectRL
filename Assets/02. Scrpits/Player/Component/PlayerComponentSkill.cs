using JetBrains.Annotations;
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
		protected List<SPlayerSkillDataSet> skillDataSets;
		protected Dictionary<int, Tuple<int, int>> skillToSkillDataMap;

		public SPlayerSkillDataSet[] SkillDataSets { get => skillDataSets.ToArray(); }

		public PlayerComponentSkill(PlayerModel model, APlayerSkillDataSO[] skillDatas)
		{
			playerModel = model;
			skills = new APlayerSkill[skillDatas.Length];
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
			activeSkill = new APlayerSkill[4];

			for (int i = 0; i < activeSkill.Length; i++)
			{
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

		public bool SetSkill(short targetIndex, string skillName)
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

			if (targetIndex == idx)
			{
				activeSkill[targetIndex].IsSelected = false;
				activeSkill[targetIndex] = null;
			}
			else
			{
				activeSkill[targetIndex].IsSelected = false;
				WriteSkillData(activeSkill[targetIndex]);
				activeSkill[targetIndex] = skill;
				activeSkill[targetIndex].IsSelected = true;
			}
			if (idx != -1)
			{
				activeSkill[idx] = null;
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
			return (activeSkill);
		}
	}
}
