using UnityEngine;

namespace Achievement
{
	public class AchievementTest : IAchievement
	{
		public string Name { get; private set; }
		public bool IsUnlocked { get; private set; }

		public AchievementTest()
		{
			Name = "Test_Achievement";
			IsUnlocked = false;
			return ;
		}

		public void Initialize(EventBus eventBus)
		{
			eventBus.Subscribe<SEventTest>(TestFunction);
			return ;
		}
		
		public void Shutdown(EventBus eventBus)
		{
			eventBus.Unsubscribe<SEventTest>(TestFunction);
			return ;
		}

		public void TestFunction(SEventTest evt)
		{
			if (IsUnlocked)
				return ;
			IsUnlocked = true;
			Debug.Log("Test Achievement Unlock!");
			return ;
		}
	}
}
