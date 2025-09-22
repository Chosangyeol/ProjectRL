using System;
using UnityEngine;
using UnitySubCore.Singleton;

namespace Config
{
	public class ConfigUserInput : ASingleton<ConfigUserInput>, IConfig
	{
		public event Action ActionCallbackConfigChanged;

		// ===== KeyCode Mapping =====
		public KeyCode	keyMoveFront = KeyCode.W;
		public KeyCode	keyMoveBack = KeyCode.S;
		public KeyCode	keyMoveLeft = KeyCode.A;
		public KeyCode	keyMoveRight = KeyCode.D;

		public KeyCode	keySprint = KeyCode.LeftShift;
		public KeyCode	keyDash = KeyCode.LeftControl;
		public KeyCode	keyJump = KeyCode.Space;

		public KeyCode	keySkill1 = KeyCode.Q;
		public KeyCode	keySkill2 = KeyCode.E;
		public KeyCode	keySkill3 = KeyCode.R;
		public KeyCode	keySkill4 = KeyCode.G;

		public KeyCode	keyPause = KeyCode.Escape;

		// ===== User Setting =====
		public bool		isAxisYFlipped = false;

		public void OnChangeConfig()
		{
			ActionCallbackConfigChanged?.Invoke();
			return ;
		}
	}
}
