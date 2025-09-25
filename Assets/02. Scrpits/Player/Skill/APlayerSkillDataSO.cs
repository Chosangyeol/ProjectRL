using UnityEngine;

namespace Player.Skill
{
	[CreateAssetMenu(fileName = "New Player Skill Data", menuName = "SO/Player Skill Data")]
	public abstract class APlayerSkillDataSO : ScriptableObject
	{
		public string skillName;
		public float coolTime;
		public string tooltip;
		public Sprite skillSprite;

		public abstract APlayerSkill CreateSKill();
	}
}
