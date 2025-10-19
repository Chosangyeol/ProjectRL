using System.Collections;
using UnityEngine;

namespace Player.Skill
{
	public class PlayerSkillBuckShot : PlayerSkill
	{
		public PlayerSkillBuckShot(PlayerSkillDataSO dataSO, int tmp) : base(dataSO)
		{
			return ;
		}

		public override IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey)
		{
			model.Shoot("BuckShot", 7f, 0f);
			yield break ;
		}
	}
}
