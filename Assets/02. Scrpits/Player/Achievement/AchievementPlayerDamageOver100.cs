using UnityEngine;

namespace Achievement
{
	public class AchievementPlayerDamageOver100 : IAchievement
	{
		public string Name { get; private set; }
		public bool IsUnlocked { get; private set; }

		public AchievementPlayerDamageOver100()
		{
			Name = "Test_Achievement";
			IsUnlocked = false;
			return ;
		}

		public void Initialize(EventBus eventBus)
		{
			eventBus.Subscribe<SEventPlayerDamage>(TestFunction);
			return ;
		}
		
		public void Shutdown(EventBus eventBus)
		{
			eventBus.Unsubscribe<SEventPlayerDamage>(TestFunction);
			return ;
		}

		public void Unlock()
		{
			IsUnlocked = true;
			Debug.Log($"Unlock {this.GetType().ToString()}");
			return ;
		}

		public void TestFunction(SEventPlayerDamage evt)
		{
			if (IsUnlocked)
				return ;
			if (evt.damage >= 100)
			{
				Unlock();
			}
			return ;
		}
	}
}
