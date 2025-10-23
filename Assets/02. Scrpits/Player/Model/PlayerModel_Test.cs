using Info;
using Player.Item;
using Player.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Skill;
using System.Reflection;

namespace Player
{
	public sealed class PlayerModel_Test : PlayerModel
	{
		[Header("Character Property")]
		[SerializeField]
		private Transform _gunParentTrans;

		protected override void Update()
		{
			base.Update();
			TurnGun();
			return ;
		}

		private void TurnGun()
		{
			_gunParentTrans.localRotation = Quaternion.Euler(cameraRotation.eulerAngles.x, 0, 0);
			return ;
		}
	}
}