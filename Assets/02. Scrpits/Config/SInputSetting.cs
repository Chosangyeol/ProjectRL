using System;
using System.Collections.Generic;
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
				{ "keyMoveFront", KeyCode.W },
				{ "keyMoveBack",  KeyCode.S },
				{ "keyMoveLeft",  KeyCode.A },
				{ "keyMoveRight", KeyCode.D },

				{ "keySprint", KeyCode.LeftShift },
				{ "keyDash",   KeyCode.LeftControl },
				{ "keyJump",   KeyCode.Space },

				{ "keySkill1", KeyCode.Q },
				{ "keySkill2", KeyCode.E },
				{ "keySkill3", KeyCode.R },
				{ "keySkill4", KeyCode.G },

				{ "keyPause", KeyCode.Escape }
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

		public KeyCode keySkill1;
		public KeyCode keySkill2;
		public KeyCode keySkill3;
		public KeyCode keySkill4;

		public KeyCode keyPause;

		// ===== User Setting =====
		public bool isAxisYFlipped;

		public void Init()
		{
			foreach (var field in typeof(SInputSetting).GetFields())
			{
				if (field.FieldType == typeof(KeyCode) && (KeyCode)field.GetValue(this) == KeyCode.None)
				{
					field.SetValueDirect(__makeref(this), GetDefaultKey()[field.Name]);
				}
			}
			return;
		}
	}
}
