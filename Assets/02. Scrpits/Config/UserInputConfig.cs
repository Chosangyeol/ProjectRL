using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Config.UserInput
{
	public class UserInputConfig : NonMonoSingleton<UserInputConfig>
	{
		// ===== KeyCode Mapping =====
		public KeyCode	keyJump;

		// ===== User Setting =====
		public bool		isAxisYFlipped = false;
	}
}
