using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Player.Component
{
	public class PlayerComponentStat
	{
		public PlayerModel playerModel;

		private SPlayerStat origin;
		private SPlayerStat edited;

		private float timeHPRegen = 1f;
		private float hpRegentimer = 0f;
		private float timeSPRegen = 1f;
		private float spRegentimer = 0f;
		private float timeSPused = 0f;

		public SPlayerStat Stat { get => edited; }

		public delegate void StatCalculator(ref SPlayerStat stat);

		private event StatCalculator	ActionCalculateStat;

		public PlayerComponentStat(PlayerModel model, PlayerComponentStatSO so)
		{
			playerModel = model;
			Equalize(so);
			return ;
		}

		public void Update(float deltaTime)
		{
			hpRegentimer += deltaTime;
			if (hpRegentimer >= timeHPRegen)
			{
				hpRegentimer -= timeHPRegen;
				Healed(1);
			}
			if (timeSPused > 0)
			{
				timeSPused -= deltaTime;
			}
			else
			{
				spRegentimer += deltaTime;
				if (spRegentimer >= timeSPRegen)
				{
					spRegentimer -= timeSPRegen;
					RegenSP(1);
				}
			}
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
			timeHPRegen = 1f / origin.hpRegenPerSecond;
			hpRegentimer = 0f;
			timeSPRegen = 1f / origin.spRegenPerSecond;
			spRegentimer = 0f;
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
			RecalculateStat();
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
			RecalculateStat();
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
			RecalculateStat();
			return (heal);
		}

		public int RegenSP(int regen)
		{
			if (regen < 0)
				return (0);
			origin.spCurrent += regen;
			if (origin.spCurrent > origin.spMax)
			{
				regen = origin.spCurrent - origin.spMax;
				origin.spCurrent = origin.spMax;
			}
			RecalculateStat();
			return (regen);
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
			RecalculateStat();
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
			origin.expCurrent += exp;
			result = LevelUp();
			RecalculateStat();
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
			origin.jumpCountCurrent++;
			return ;
		}

		public void ResetJumpCount()
		{
			edited.jumpCountCurrent = 0;
			origin.jumpCountCurrent = 0;
			return ;
		}

		public bool CanJump()
		{
			return (edited.jumpCountCurrent < edited.jumpCountMax);
		}

		public float GetSpeed(bool isSprint)
		{
			if (isSprint && Stat.spCurrent > 0)
			{
				timeSPused = 3f;
				origin.spCurrent -= 1;
				return (edited.speedSprint);
			}
			return (edited.speedMove);
		}

		public float GetJumpPower()
		{
			return (edited.jumpPower);
		}

		private void Equalize(PlayerComponentStatSO so)
		{
			origin.Equalize(so);
			RecalculateStat();
			return ;
		}
	}
}
