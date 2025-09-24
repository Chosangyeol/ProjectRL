using System;
using Unity.VisualScripting;

namespace Player.Component
{
	public class PlayerComponentStat
	{
		public PlayerModel playerModel;

		private SPlayerStat origin;
		public SPlayerStat stat;

		public delegate void StatCalculator(ref SPlayerStat stat);
		public event StatCalculator ActionCalculateStat;

		public PlayerComponentStat(PlayerModel model, PlayerComponentStatSO so)
		{
			playerModel = model;
			playerModel.ActionCallbackSkillChanged += () => RecalculateStat();
			playerModel.ActionCallbackBuffChanged += () => RecalculateStat();
			Equalize(so);
			return ;
		}

		public void RecalculateStat()
		{
			stat = origin;
			ActionCalculateStat?.Invoke(ref stat);
			return ;
		}

		public void AddStat(SPlayerStat add)
		{
			origin += add;
			RecalculateStat();
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
			}
			stat.shield = origin.shield;
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
			stat.shield = origin.shield;
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
			stat.hpCurrent = origin.hpCurrent;
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
			stat.hpCurrent = origin.hpCurrent;
			return (damage);
		}

		public bool IsAlive()
		{
			return (stat.hpCurrent > 0);
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
			stat.levelCurrent = origin.levelCurrent;
			stat.expCurrent = origin.expCurrent;
			stat.expMax = origin.expMax;
			stat.expExtendWhenLevelUp = origin.expExtendWhenLevelUp;
			return (result);
		}

		public void CountJump()
		{
			stat.jumpCountCurrent++;
			return ;
		}

		public void ResetJumpCount()
		{
			stat.jumpCountCurrent = 0;
			return ;
		}

		public bool CanJump()
		{
			return (stat.jumpCountCurrent < stat.jumpCountMax);
		}

		public float GetSpeed(bool isSprint)
		{
			if (isSprint)
				return (stat.speedSprint);
			return (stat.speedMove);
		}

		public float GetJumpPower()
		{
			return (stat.jumpPower);
		}

		private void Equalize(PlayerComponentStatSO so)
		{
			origin.Equalize(so);
			stat = origin;
			return ;
		}
	}
}
