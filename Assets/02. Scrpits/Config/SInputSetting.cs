using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

namespace Config
{
	[Serializable]
	public struct SInputSetting
	{
		public Dictionary<string, KeyCode> GetDefaultKey()
		{
			return (new Dictionary<string, KeyCode>
			{
				{ nameof(keyMoveFront), KeyCode.W },
				{ nameof(keyMoveBack),  KeyCode.S },
				{ nameof(keyMoveLeft),  KeyCode.A },
				{ nameof(keyMoveRight), KeyCode.D },

				{ nameof(keySprint), KeyCode.LeftShift },
				{ nameof(keyDash),   KeyCode.LeftControl },
				{ nameof(keyJump),   KeyCode.Space },

				{ nameof(keyInteract), KeyCode.F },
				{ nameof(keyInventory), KeyCode.I },
				{ nameof(keySkillTree), KeyCode.K },

				{ nameof(keySkill1), KeyCode.Q },
				{ nameof(keySkill2), KeyCode.E },
				{ nameof(keySkill3), KeyCode.R },
				{ nameof(keySkill4), KeyCode.G },

				{ nameof(keyPause), KeyCode.Escape }
			});
		}

		// ===== KeyCode Mapping =====
		public KeyCode keyMoveFront;
		public KeyCode keyMoveBack;
		public KeyCode keyMoveLeft;
		public KeyCode keyMoveRight;

		public KeyCode keySprint;
		public KeyCode keyDash;
		public KeyCode keyJump;

		public KeyCode keyInteract;
		public KeyCode keyInventory;
		public KeyCode keySkillTree;

		public KeyCode keySkill1;
		public KeyCode keySkill2;
		public KeyCode keySkill3;
		public KeyCode keySkill4;

		public KeyCode keyPause;

		// ===== User Setting =====
		public bool isAxisYFlipped;

		public void Init(ref SInputSetting self)
		{
			Dictionary<string, KeyCode> defaultKeys = GetDefaultKey();
			FieldInfo[] fields = typeof(SInputSetting).GetFields(BindingFlags.Public | BindingFlags.Instance);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];
				if (field.FieldType != typeof(KeyCode))
					continue ;

				KeyCode currentValue = (KeyCode)field.GetValue(self);
				if (currentValue == KeyCode.None && defaultKeys.TryGetValue(field.Name, out var defaultValue))
				{
					field.SetValueDirect(__makeref(self), defaultValue);
				}
			}
			return;
		}
	}
}
