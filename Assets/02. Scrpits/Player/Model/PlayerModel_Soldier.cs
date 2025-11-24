using JetBrains.Annotations;
using Player.Component;
using System;
using UnityEngine;

namespace Player
{
	public sealed class PlayerModel_Soldier : PlayerModel
	{
		[Header("Animation IK positions")]
		[SerializeField]
		private Transform _gunParentTrans;
		[SerializeField]
		private Transform _leftHandTr;
		[SerializeField]
		private Transform _rightHandTr;

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
		protected override void Update()
		{
			base.Update();
			TurnGun();
			return;
		}

		private void TurnGun()
		{
			_gunParentTrans.localRotation = Quaternion.Euler(cameraRotation.eulerAngles.x, 0, 0);
			return ;
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

		private void OnAnimatorIK(int layerIndex)
		{
			if (layerIndex != 0)
				return ;

			cpnAnimation.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
			cpnAnimation.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

			cpnAnimation.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTr.position);
			cpnAnimation.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTr.rotation);

			cpnAnimation.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
			cpnAnimation.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

			cpnAnimation.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTr.position);
			cpnAnimation.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTr.rotation);

			cpnAnimation.SetLookAtWeight(1f);
			cpnAnimation.SetLookAtPosition(raycaster.GetRaycastHitPoint());
			return ;
		}
	}
}