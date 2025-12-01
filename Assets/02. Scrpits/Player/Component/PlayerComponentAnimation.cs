using UnityEngine;

namespace Player.Component
{
	public class PlayerComponentAnimation
	{
		public PlayerModel playerModel;
		public Animator animator;

		public PlayerComponentAnimation(PlayerModel model)
		{
			playerModel = model;
			animator = playerModel.GetComponent<Animator>();
			return ;
		}

		public void Update(float delta)
		{
			return ;
		}
	}
}
