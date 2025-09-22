using JetBrains.Annotations;
using System;

namespace Player
{
	public class PlayerComponentStat
	{
		public int hpMax;
		public int hpCurrent;
		public int hpRegenPerSecond;
		public int shield;

		public int spMax;
		public int spCurrent;
		public int spRegenPerSecond;

		public int levelCurrent;
		public int levelMax;
		public int expCurrent;
		public int expMax;
		public int expExtendWhenLevelUp;

		public float speedMove;
		public float speedSprint;
		public float powerDash;

		public int jumpCountCurrent;
		public int jumpCountMax;
		public float jumpPower;

		public float attackDamage;
		public float critPercent;
		public float critDamagePercent;

		public PlayerComponentStat(PlayerComponentStatSO so)
		{
			Equalize(so);
			return ;
		}

		public int AddShield(int add)
		{
			if (add < 0)
				return (0);
			shield += add;
			if (shield > Int32.MaxValue)
			{
				add = shield - Int32.MaxValue;
				shield = Int32.MaxValue;
			}
			return (add);
		}

		public int RemoveShield(int remove)
		{
			if (shield <= 0 || remove <= 0)
				return (remove);
			shield -= remove;
			if (shield < 0)
			{
				remove += shield;
				shield = 0;
			}
			return (remove);
		}

		public int Healed(int heal)
		{
			if (heal < 0)
				return (0);
			hpCurrent += heal;
			if (hpCurrent > hpMax)
			{
				heal = hpCurrent - hpMax;
				hpCurrent = hpMax;
			}
			return (heal);
		}

		public int Damaged(int damage)
		{
			if (damage <= 0)
				return (0);
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
			hpCurrent = Math.Max(hpCurrent - damage, 0);
			return (damage);
		}

		public bool IsAlive()
		{
			return (hpCurrent > 0);
		}

		public int AddExp(int exp)
		{
			int result = 0;

			if (levelCurrent >= levelMax)
				return (result);
			result = LevelUp();
			return (result);
		}

		private int LevelUp()
		{
			int result = 0;

			while (expCurrent >= expMax)
			{
				if (levelCurrent >= levelMax)
					break ;
				levelCurrent++;
				expCurrent -= expMax;
				expMax += expExtendWhenLevelUp;
				result++;
			}
			return (result);
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

		private void Equalize(PlayerComponentStatSO so)
		{
			hpCurrent = hpMax = so.hpMax;
			hpRegenPerSecond = so.hpRegenPerSecond;
			shield = 0;
			spCurrent = spMax = so.spMax;
			spRegenPerSecond = so.spRegenPerSecond;
			levelCurrent = 1;
			levelMax = so.levelMax;
			expCurrent = 0;
			expMax = so.expMax;
			expExtendWhenLevelUp = so.expExtendWhenLevelUp;
			speedMove = so.speedMove;
			speedSprint = so.speedSprint;
			powerDash = so.powerDash;
			jumpCountCurrent = 0;
			jumpCountMax = so.jumpCountMax;
			jumpPower = so.jumpPower;
			attackDamage = so.attackDamage;
			critPercent = so.critPercent;
			critDamagePercent = so.critDamagePercent;
			return ;
		}
	}
}
