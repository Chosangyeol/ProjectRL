using UnityEngine;

namespace Player.Buff
{
	public abstract class ABuffPlayer : IBuffAct
	{
		protected PlayerModel target;
		protected float remainSecond;
		public bool canDiscount;
		
		public BuffType Type { get; protected set; }
		public string Desc { get; protected set; }

		public ABuffPlayer(PlayerModel target, float remainSecond, string desc)
		{
			this.target = target;
			this.remainSecond = remainSecond;
			canDiscount = true;
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

		public void SetUnactive()
		{
			remainSecond = 0f;
			return ;
		}
	}
}
