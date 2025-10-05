using System;
using UnityEngine;

namespace Player.Skill
{
	public abstract class APlayerSkillDataSO : ScriptableObject
	{
		public string skillName;
		public float coolTime;
		public string tooltip;
		public Sprite skillSprite;

		public abstract APlayerSkill CreateSkill();
	}

	// !TODO
	[CreateAssetMenu(fileName = "New Player Skill Data", menuName = "SO/Player Skill Data")]
	public class PlayerSkillDataSO : APlayerSkillDataSO
	{
		public override APlayerSkill CreateSkill()
		{
			throw (new NotImplementedException());
		}
	}
}
