using Achievement;
using UnityEngine;

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

		public override void Shoot(Vector3 targetPos, string name, float speed = 5, float spread = 0.04F)
		{
			base.Shoot(targetPos, name, speed, spread);
			AchievementSystem.Instance.Publish(new SEventTest());
			return ;
		}
	}
}