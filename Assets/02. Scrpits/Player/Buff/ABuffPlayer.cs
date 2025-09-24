using UnityEngine;

namespace Player.Buff
{
	public abstract class ABuffPlayer : IBuffAct
	{
		private PlayerModel target;
		private float remainSecond;
		public bool canDiscount;

		public GameObject Target { get; private set; }
		public string Desc { get; protected set; }

		public ABuffPlayer(PlayerModel target, float remainSecond, string desc)
		{
			Target = target.gameObject;
			this.remainSecond = remainSecond;
			canDiscount = true;
			this.target = target;
			Desc = desc;
			return ;
		}

		public abstract void OnEnable();

		public virtual bool Update(float delta)
		{
			if (canDiscount)
				remainSecond -= delta;
			return (remainSecond <= 0f);
		}

		public abstract void OnDisable();

		public void SetDisable()
		{
			remainSecond = 0f;
			return ;
		}
	}
}
