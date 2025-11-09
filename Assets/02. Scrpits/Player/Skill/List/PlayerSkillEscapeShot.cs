using Player.Component;
using System.Collections;
using UnityEngine;

namespace Player.Skill
{
	public class PlayerSkillEscapeShot : PlayerSkill
	{
		private float editSpeed = 5f;
		private float holdMaxTime = 5f;
		private int shootBullet = 5;

		public PlayerSkillEscapeShot(PlayerSkillDataSO dataSO, int tmp) : base(dataSO)
		{
			return ;
		}

		public override IEnumerator SkillCoroutine(PlayerModel model, KeyCode skillKey)
		{
			float timer = 0f;
			float lastShotTime = 0f;
			int shootCount = 0;

			model.Stat.EditOriginStat(AddStat);
			while (timer < holdMaxTime)
			{
				if (!Input.GetKey(skillKey) || canUse)
				{
					break ;
				}
				if (shootCount < shootBullet && timer >= lastShotTime)
				{
					model.Shoot("EscapeShot", 10, 0.02f);
					lastShotTime += 0.1f;
					shootCount++;
				}
				yield return (null);
				timer += Time.deltaTime;
			}
			model.Stat.EditOriginStat(RemoveStat);
			Debug.Log("Stop EscapeShot");
			yield break ;
		}

		public void AddStat(ref SPlayerStat stat)
		{
			stat.speedMove += editSpeed;
			stat.speedSprint += editSpeed;
			return ;
		}

		public void RemoveStat(ref SPlayerStat stat)
		{
			stat.speedMove -= editSpeed;
			stat.speedSprint -= editSpeed;
			return;
		}
	}
}
