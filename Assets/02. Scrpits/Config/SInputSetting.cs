using System;
using UnityEngine;

namespace Config
{
	[Serializable]
	public struct SInputSetting
	{
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
			// ===== KeyCode Mapping =====
			keyMoveFront = KeyCode.W;
			keyMoveBack = KeyCode.S;
			keyMoveLeft = KeyCode.A;
			keyMoveRight = KeyCode.D;

			keySprint = KeyCode.LeftShift;
			keyDash = KeyCode.LeftControl;
			keyJump = KeyCode.Space;

			keySkill1 = KeyCode.Q;
			keySkill2 = KeyCode.E;
			keySkill3 = KeyCode.R;
			keySkill4 = KeyCode.G;
			keyPause = KeyCode.Escape;

			// ===== User Setting =====
			isAxisYFlipped = false;
		}
	}
}
