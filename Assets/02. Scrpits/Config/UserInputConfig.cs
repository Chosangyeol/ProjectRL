using UnityEngine;
using UnitySubCore.Singleton;

namespace Config.UserInput
{
	public class UserInputConfig : ASingleton<UserInputConfig>
	{
		// ===== KeyCode Mapping =====
		public KeyCode	keyJump;

		// ===== User Setting =====
		public bool		isAxisYFlipped = false;
	}
}
