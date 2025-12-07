using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Skill
{
	public abstract class APlayerSkill
	{
		public APlayerSkillDataSO dataSO;

		protected bool isSelected;
		protected float coolTime;
		protected float nowTime;
		protected bool canUse;

		public bool IsSelected { get => isSelected; set => isSelected = value; }

		public APlayerSkill(APlayerSkillDataSO dataSO)
		{
			this.dataSO = dataSO;
			this.coolTime = dataSO.coolTime;
			isSelected = false;
			nowTime = 0f;
			canUse = true;
			return ;
		}

		public virtual bool UseSkill(PlayerModel model, KeyCode skillKey)
		{
			if (canUse)
			{
				nowTime = coolTime;
				canUse = false;
				Activate(model, skillKey);
				return (true);
			}
			return (false);
		}

		public virtual void Activate(PlayerModel model, KeyCode skillKey)
		{
			model.StartCoroutine(SkillCoroutine(model, skillKey));
			return ;
		}

		public virtual void UpdateSkill(float delta)
		{
			if (canUse)
				return ;
			nowTime -= delta;
			canUse = (nowTime < 0f);
			return ;
		}

		public abstract IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey);
	}

	public class PlayerSkill : APlayerSkill
	{
		public static readonly Dictionary<string, Type> skillTypes = new Dictionary<string, Type>()
		{
			{ "PlayerSkillDash", typeof(PlayerSkillDash)},
			{ "PlayerSkillBuckShot", typeof(PlayerSkillBuckShot)},
			{ "PlayerSkillEscapeShot", typeof(PlayerSkillEscapeShot)},
			{ "PlayerSkillFaintGrenade", typeof(PlayerSkillFaintGrenade)},
		};

		public PlayerSkill(PlayerSkillDataSO dataSO) : base(dataSO)
		{
			return ;
		}

		public override IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey)
		{
			Debug.Log($"if you see this log, something is wrong in PlayerSkill");
			yield break ;
		}
	}
}
