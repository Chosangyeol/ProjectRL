using System;

namespace Player
{
	[Serializable]
	public class PlayerComponentStat
	{
		public int hpCurrent;
		public int hpMax;
		public int shield;

		public int levelCurrent;
		public int levelMax;
		public int expCurrent;
		public int expMax;
		
		public float speedWalk;
		public float speedRush;
		public float speedDash;

		public int jumpCountCurrent;
		public int jumpCountMax;
		public float jumpPower;

		public float critPercent;
		public float critDamagePercent;

		public PlayerComponentStat()
		{

			return ;
		}

		// TODO!
		public bool Healed(int heal)
		{
			if (heal < 0)
				return (false);
			hpCurrent = Math.Min(hpCurrent + heal, hpMax);
			return (true);
		}

		public bool Damaged(int damage)
		{
			if (damage < 0)
				return (false);
			if (shield <= damage)
			{
				damage -= shield;
				shield = 0;
			}
			else
			{
				shield -= damage;
				damage = 0;
			}
			hpCurrent -= damage;
			return (true);
		}

		public bool IsAlive()
		{
			return (hpCurrent > 0);
		}

		public void CountJump()
		{
			jumpCountCurrent++;
			return ;
		}

		public void ResetJumpCount()
		{
			jumpCountCurrent = 0;
			return ;
		}

		public bool CanJump()
		{
			return (jumpCountCurrent < jumpCountMax);
		}
	}
}
