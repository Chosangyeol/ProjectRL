using Player.Component;
using System;
using UnityEngine;

namespace Player
{
	public sealed class PlayerModel_Soldier : PlayerModel
	{
		protected override void Awake()
		{
			base.Awake();
			cpnSkill = new PlayerSkillSoldierComponent(this, _skillDataSO);
			cpnAnimation = new PlayerAnimationSoldierComponent(this);
			return ;
		}

		public override bool Attack(Vector3 targetPos)
		{
			bool result = base.Attack(targetPos);

			if (result)
			{
				cpnAnimation.SetTrigger("tShooting");
			}
			return (result);
		}

		public override Vector3 Move(Transform parent, Vector3 movement, bool isSprint, Action callback = null)
		{
			Vector3 result = base.Move(parent, movement, isSprint, callback);

			if (result == Vector3.zero)
			{
				cpnAnimation.SetFloat("fsqrSpeed", 0f);
				return (result);
			}
			cpnAnimation.SetFloat("fsqrSpeed", 1f);
			cpnAnimation.SetBool("bMoveBack", Vector3.Dot(transform.forward, movement) < 0f);
			return (result);
		}
	}
}