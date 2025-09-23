using System;
using Unity.VisualScripting;

namespace Player.Component
{
	public class PlayerComponentStat
	{
		public PlayerModel playerModel;

		private SPlayerStat origin;
		public SPlayerStat stat;

		public PlayerComponentStat(PlayerModel model, PlayerComponentStatSO so)
		{
			playerModel = model;
			Equalize(so);
			return ;
		}

		public void AddStat(SPlayerStat add)
		{
			origin += add;
			return ;
		}

		public int AddShield(int add)
		{
			if (add < 0)
				return (0);
			origin.shield += add;
			if (origin.shield > Int32.MaxValue)
			{
				add = origin.shield - Int32.MaxValue;
				stat.shield = origin.shield = Int32.MaxValue;
			}
			return (add);
		}

		public int RemoveShield(int remove)
		{
			if (origin.shield <= 0 || remove <= 0)
				return (remove);
			origin.shield -= remove;
			if (origin.shield < 0)
			{
				remove += origin.shield;
				origin.shield = 0;
			}
			return (remove);
		}

		public int Healed(int heal)
		{
			if (heal < 0)
				return (0);
			origin.hpCurrent += heal;
			if (origin.hpCurrent > origin.hpMax)
			{
				heal = origin.hpCurrent - origin.hpMax;
				origin.hpCurrent = origin.hpMax;
			}
			return (heal);
		}

		public int Damaged(int damage)
		{
			if (damage <= 0)
				return (0);
			if (origin.shield <= damage)
			{
				damage -= origin.shield;
				origin.shield = 0;
			}
			else
			{
				origin.shield -= damage;
				damage = 0;
			}
			origin.hpCurrent = Math.Max(origin.hpCurrent - damage, 0);
			return (damage);
		}

		public bool IsAlive()
		{
			return (origin.hpCurrent > 0);
		}

		public int AddExp(int exp)
		{
			int result = 0;

			if (origin.levelCurrent >= origin.levelMax)
				return (result);
			result = LevelUp();
			return (result);
		}

		private int LevelUp()
		{
			int result = 0;

			while (origin.expCurrent >= origin.expMax)
			{
				if (origin.levelCurrent >= origin.levelMax)
					break ;
				origin.levelCurrent++;
				origin.expCurrent -= origin.expMax;
				origin.expMax += origin.expExtendWhenLevelUp;
				result++;
			}
			return (result);
		}

		public void CountJump()
		{
			origin.jumpCountCurrent++;
			return ;
		}

		public void ResetJumpCount()
		{
			origin.jumpCountCurrent = 0;
			return ;
		}

		public bool CanJump()
		{
			return (origin.jumpCountCurrent < origin.jumpCountMax);
		}

		private void Equalize(PlayerComponentStatSO so)
		{
			origin.Equalize(so);

			return ;
		}
	}
}
