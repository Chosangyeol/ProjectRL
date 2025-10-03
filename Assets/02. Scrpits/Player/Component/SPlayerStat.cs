using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq.Expressions;

namespace Player.Component
{
	public struct SPlayerStat
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

		public int attackDamage;
		public float critPercent;
		public float critDamagePercent;

		//#region Reflection

		//private delegate void SetHandler<GType01>(ref SPlayerStat stat, GType01 v);
		//private static readonly Dictionary<string, Func<SPlayerStat, object>> getters;
		//private static readonly Dictionary<string, Delegate> setters;

		//static SPlayerStat()
		//{
		//	FieldInfo[] fields = typeof(SPlayerStat).GetFields(BindingFlags.Public | BindingFlags.Instance);

		//	getters = new Dictionary<string, Func<SPlayerStat, object>>();
		//	setters = new Dictionary<string, Delegate>();
		//	for (int i = 0; i < fields.Length; i++)
		//	{
		//		FieldInfo field = fields[i];

		//		ParameterExpression param = Expression.Parameter(typeof(SPlayerStat), "s");
		//		MemberExpression fieldAccess = Expression.Field(param, field);
		//		UnaryExpression convert = Expression.Convert(fieldAccess, typeof(object));
		//		getters[field.Name] = Expression.Lambda<Func<SPlayerStat, object>>(convert, param).Compile();

		//		ParameterExpression structParam = Expression.Parameter(typeof(SPlayerStat).MakeByRefType(), "s");
		//		ParameterExpression valueParam = Expression.Parameter(field.FieldType, "value");
		//		BinaryExpression assign = Expression.Assign(Expression.Field(structParam, field), valueParam);
		//		LambdaExpression lambdaSetter = Expression.Lambda(assign, structParam, valueParam);
		//		setters[field.Name] = lambdaSetter.Compile();
		//	}
		//}

		//public GType01 Get<GType01>(string name)
		//{
		//	if (getters.TryGetValue(name, out var getter))
		//		return ((GType01)getter(this));
		//	throw new ArgumentException($"{name} is not a valid field");
		//}

		//public void Set<GType01>(string name, GType01 value)
		//{
		//	if (setters.TryGetValue(name, out var setter))
		//	{
		//		if (setter is SetHandler<GType01> typedSetter)
		//		{
		//			typedSetter(ref this, value);
		//			return;
		//		}
		//		else
		//		{
		//			var action = (SetHandler<GType01>)Delegate.CreateDelegate(typeof(SetHandler<GType01>), setter.Target, setter.Method);
		//			action(ref this, value);
		//			return;
		//		}
		//	}
		//	throw new ArgumentException($"{name} is not a valid field");
		//}

		//#endregion

		public void Equalize(PlayerComponentStatSO so)
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

		public static SPlayerStat operator + (SPlayerStat first, SPlayerStat second)
		{
			SPlayerStat result = new SPlayerStat();

			result.hpCurrent = first.hpCurrent + second.hpCurrent;
			result.hpMax = first.hpMax + second.hpMax;
			result.hpRegenPerSecond = first.hpRegenPerSecond + second.hpRegenPerSecond;
			result.shield = first.shield + second.shield;
			result.spCurrent = first.spCurrent + second.spCurrent;
			result.spMax = first.spMax + second.spMax;
			result.spRegenPerSecond = first.spRegenPerSecond + second.spRegenPerSecond;
			result.levelCurrent = first.levelCurrent + second.levelCurrent;
			result.levelMax = first.levelMax + second.levelMax;
			result.expCurrent = first.expCurrent + second.expCurrent;
			result.expMax = first.expMax + second.expMax;
			result.expExtendWhenLevelUp = first.expExtendWhenLevelUp + second.expExtendWhenLevelUp;
			result.speedMove = first.speedMove + second.speedMove;
			result.speedSprint = first.speedSprint + second.speedSprint;
			result.powerDash = first.powerDash + second.powerDash;
			result.jumpCountCurrent = first.jumpCountCurrent + second.jumpCountCurrent;
			result.jumpCountMax = first.jumpCountMax + second.jumpCountMax;
			result.jumpPower = first.jumpPower + second.jumpPower;
			result.attackDamage = first.attackDamage + second.attackDamage;
			result.critPercent = first.critPercent + second.critPercent;
			result.critDamagePercent = first.critPercent * second.critDamagePercent;
			return (result);
		}
	}
}
