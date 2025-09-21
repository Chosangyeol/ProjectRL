using UnityEngine;
using UnitySubCore.Singleton;

namespace Config.UserInput
{
	public class ConfigUserInput : ASingleton<ConfigUserInput>
	{
		// ===== KeyCode Mapping =====
		public KeyCode	keyJump;

		// ===== User Setting =====
		public bool		isAxisYFlipped = false;
	}
}
