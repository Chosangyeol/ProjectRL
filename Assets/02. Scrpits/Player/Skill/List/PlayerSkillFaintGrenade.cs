using System.Collections;
using UnityEngine;

namespace Player.Skill
{
	public class PlayerSkillFaintGrenade : PlayerSkill
	{
		public PlayerSkillFaintGrenade(PlayerSkillDataSO dataSO, int tmp) : base(dataSO)
		{
			return ;
		}

		public override IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey)
		{
			model.Summon("FaintGrenade", 7f, 0f);
			yield break ;
		}
	}
}
