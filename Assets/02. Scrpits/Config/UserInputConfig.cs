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
		public KeyCode jump;
		public bool isAxisYFlipped = false;
	}
}
