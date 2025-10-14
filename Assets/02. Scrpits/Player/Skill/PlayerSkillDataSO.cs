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

	[CreateAssetMenu(fileName = "New Player Skill Data", menuName = "SO/Player Skill Data")]
	public class PlayerSkillDataSO : APlayerSkillDataSO
	{
		[SerializeField, SerializeReference]
		private string skillClassName;

		public override APlayerSkill CreateSkill()
		{
			Type type = PlayerSkill.skillTypes[skillClassName];

			return ((APlayerSkill)Activator.CreateInstance(type, this, 0));
		}
	}
}
