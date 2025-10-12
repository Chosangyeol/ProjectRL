using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Player.Skill
{
	public abstract class APlayerSkill
	{
		public APlayerSkillDataSO dataSO;
		private float coolTime;
		private float nowTime;
		private bool canUse;

		public APlayerSkill(APlayerSkillDataSO dataSO)
		{
			this.dataSO = dataSO;
			this.coolTime = dataSO.coolTime;
			nowTime = 0f;
			canUse = true;
			return ;
		}

		public virtual bool UseSkill(PlayerModel model)
		{
			if (canUse)
			{
				Activate(model);
				canUse = false;
				nowTime = coolTime;
				return (true);
			}
			return (false);
		}

		public abstract void Activate(PlayerModel model);

		public virtual void UpdateSkill(float delta)
		{
			if (canUse)
				return ;
			nowTime -= delta;
			canUse = (nowTime < 0f);
			return ;
		}
	}

	public class PlayerSkill : APlayerSkill
	{
		public static readonly Dictionary<string, Type> skillTypes = new Dictionary<string, Type>()
		{
			{ "PlayerSkillDash", typeof(PlayerSkillDash)},
		};

		public PlayerSkill(PlayerSkillDataSO dataSO) : base(dataSO)
		{
			return ;
		}

		public override void Activate(PlayerModel model)
		{
			return ;
		}
	}
}
