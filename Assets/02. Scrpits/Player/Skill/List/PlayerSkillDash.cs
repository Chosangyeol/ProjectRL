using UnityEngine.Rendering;

namespace Player.Skill
{
	public class PlayerSkillDash : PlayerSkill
	{
		public PlayerSkillDash(PlayerSkillDataSO dataSO, int tmp) : base(dataSO)
		{
			return ;
		}

		public override void Activate(PlayerModel model)
		{
			model.Dash();
			return ;
		}
	}
}
