using System;
using System.Security.Cryptography;

namespace Player.Component
{
	public class PlayerComponentStat
	{
		public PlayerModel playerModel;

		private SPlayerStat origin;
		private SPlayerStat edited;

		public SPlayerStat Stat { get => edited; }

		public delegate void StatCalculator(ref SPlayerStat stat);

		private event StatCalculator	ActionCalculateStat;

		public PlayerComponentStat(PlayerModel model, PlayerComponentStatSO so)
		{
			playerModel = model;
			Equalize(so);
			return ;
		}

		public void EditOriginStat(StatCalculator calculator)
		{
			calculator(ref origin);
			RecalculateStat();
			return ;
		}

		public void AddCalculateStat(StatCalculator calculator)
		{
			ActionCalculateStat += calculator;
			RecalculateStat();
			return ;
		}

		public void RemoveCalculateStat(StatCalculator calculator)
		{
			ActionCalculateStat -= calculator;
			RecalculateStat();
			return ;
		}

		public void AddStat(SPlayerStat add)
		{
			origin += add;
			RecalculateStat();
			return ;
		}

		private void RecalculateStat()
		{
			edited = origin;
			ActionCalculateStat?.Invoke(ref edited);
			return ;
		}

		public int AddShield(int add)
		{
			if (add < 0)
				return (0);
			edited.shield += add;
			if (edited.shield > Int32.MaxValue)
			{
				add = edited.shield - Int32.MaxValue;
			}
			return (add);
		}

		public int RemoveShield(int remove)
		{
			if (edited.shield <= 0 || remove <= 0)
				return (remove);
			edited.shield -= remove;
			if (edited.shield < 0)
			{
				remove += edited.shield;
				edited.shield = 0;
			}
			return (remove);
		}

		public int Healed(int heal)
		{
			if (heal < 0)
				return (0);
			edited.hpCurrent += heal;
			if (edited.hpCurrent > edited.hpMax)
			{
				heal = edited.hpCurrent - edited.hpMax;
				edited.hpCurrent = edited.hpMax;
			}
			return (heal);
		}

		public int Damaged(int damage)
		{
			if (damage <= 0)
				return (0);
			if (edited.shield <= damage)
			{
				damage -= edited.shield;
				edited.shield = 0;
			}
			else
			{
				edited.shield -= damage;
				damage = 0;
			}
			edited.hpCurrent = Math.Max(edited.hpCurrent - damage, 0);
			return (damage);
		}

		public bool IsAlive()
		{
			return (edited.hpCurrent > 0);
		}

		public int AddExp(int exp)
		{
			int result = 0;

			if (origin.levelCurrent >= origin.levelMax)
				return (result);
			origin.expCurrent += exp;
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
			edited.jumpCountCurrent++;
			return ;
		}

		public void ResetJumpCount()
		{
			edited.jumpCountCurrent = 0;
			return ;
		}

		public bool CanJump()
		{
			return (edited.jumpCountCurrent < edited.jumpCountMax);
		}

		public float GetSpeed(bool isSprint)
		{
			if (isSprint)
				return (edited.speedSprint);
			return (edited.speedMove);
		}

		public float GetJumpPower()
		{
			return (edited.jumpPower);
		}

		private void Equalize(PlayerComponentStatSO so)
		{
			origin.Equalize(so);
			edited = origin;
			return ;
		}
	}
}
