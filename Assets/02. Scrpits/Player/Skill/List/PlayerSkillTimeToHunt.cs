using Info;
using System.Collections;
using UnityEngine;

namespace Player.Skill
{
	public class PlayerSkillTimeToHunt : PlayerSkill
	{
		private BuffType type;

		public PlayerSkillTimeToHunt(PlayerSkillDataSO dataSO, int tmp) : base(dataSO)
		{
			type = RegistryBuff.Get("BuffTimeToHunt");
			return ;
		}

		public override IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey)
		{
			SInfoBuff info = new SInfoBuff(
				model.gameObject,
				model.gameObject,
				new PlayerBuffTimeToHunt(
					model,
					15f,
					null,
					type
					)
				);
			model.AddBuff(info);
			Debug.Log("Activate TimeToHunt");
			yield break ;
		}
	}
}
