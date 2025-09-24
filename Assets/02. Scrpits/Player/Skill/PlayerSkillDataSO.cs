using UnityEngine;

namespace Player.Skill
{
	[CreateAssetMenu(fileName = "New Player Skill Data", menuName = "SO/Player Skill Data")]
	public class PlayerSkillDataSO : ScriptableObject
	{
		public string skillName;
		public string tooltip;
		public Sprite skillSprite;
	}
}
