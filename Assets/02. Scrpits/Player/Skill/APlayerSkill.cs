using UnityEngine.Rendering;

namespace Player.Skill
{
	public abstract class APlayerSkill
	{
		public PlayerSkillDataSO dataSO;
		private float coolTime;
		private float nowTime;
		private bool canUse;

		public APlayerSkill(PlayerSkillDataSO dataSO, float coolTime)
		{
			this.dataSO = dataSO;
			this.coolTime = coolTime;
			nowTime = 0f;
			canUse = true;
			return ;
		}

		public bool UseSkill(PlayerModel model)
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

		public void UpdateSkill(float delta)
		{
			if (canUse)
				return ;
			nowTime -= delta;
			canUse = (nowTime < 0f);
			return ;
		}
	}
}
