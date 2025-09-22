using System;

namespace Config
{
	public interface IConfig
	{
		public event Action ActionCallbackConfigChanged;

		public void OnChangeConfig();
	}
}
