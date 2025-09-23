using UnityEngine;

namespace Player.Component
{
	[CreateAssetMenu(fileName = "New Player Stat", menuName = "SO/Player Stat")]
	public class PlayerComponentStatSO : ScriptableObject
	{
		[Header("Health Point")]
		public int hpMax;
		public int hpRegenPerSecond;

		[Header("Stemina Point")]
		public int spMax;
		public int spRegenPerSecond;

		[Header("Leveling")]
		public int levelMax;
		public int expMax;
		public int expExtendWhenLevelUp;
		
		[Header("Move Speed")]
		public float speedMove;
		public float speedSprint;
		public float powerDash;

		[Header("Jump")]
		public int jumpCountMax;
		public float jumpPower;

		[Header("Attack")]
		public float attackDamage;
		public float critPercent;
		public float critDamagePercent;
	}
}
