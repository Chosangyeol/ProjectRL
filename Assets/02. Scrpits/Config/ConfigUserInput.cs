using System;
using UnityEngine;
using UnitySubCore.Singleton;

namespace Config
{
	public class ConfigUserInput : ASingleton<ConfigUserInput>, IConfig
	{
		public event Action ActionCallbackConfigChanged;

		// ===== KeyCode Mapping =====
		public KeyCode	keySkill1 = KeyCode.Q;
		public KeyCode	keySkill2 = KeyCode.E;
		public KeyCode	keySkill3 = KeyCode.R;
		public KeyCode	keySkill4 = KeyCode.G;
		public KeyCode	keyJump = KeyCode.Space;

		// ===== User Setting =====
		public bool		isAxisYFlipped = false;

		public void OnChangeConfig()
		{
			ActionCallbackConfigChanged?.Invoke();
			return ;
		}
	}
}
