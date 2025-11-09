using System.Collections;
using UnityEngine;

namespace Player.Skill
{
	public class PlayerSkillDash : PlayerSkill
	{
		public PlayerSkillDash(PlayerSkillDataSO dataSO, int tmp) : base(dataSO)
		{
			return ;
		}

		public override IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey)
		{
			model.Dash();
			yield break ;
		}
	}
}
