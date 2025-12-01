using UnityEngine;

namespace Achievement
{
	public interface IAchievement
	{
		public string Name { get; }
		public bool IsUnlocked { get; }
		public void Initialize(EventBus eventBus);
		public void Shutdown(EventBus eventBus);
	}
}
