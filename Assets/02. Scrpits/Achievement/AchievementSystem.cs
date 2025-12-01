using System;
using System.Collections.Generic;
using UnitySubCore.Singleton;

namespace Achievement
{
	public class AchievementSystem : ASingleton<AchievementSystem>
	{
		private List<IAchievement> achievements;
		private EventBus eventbus;

		public AchievementSystem()
		{
			achievements = new List<IAchievement>();
			eventbus = new EventBus();
			Initailize();
			return;
		}

		public void Initailize()
		{
			achievements.Clear();
			foreach (IAchievement achievement in achievements)
			{
				achievement.Initialize(eventbus);
			}
			return ;
		}

		public void Shutdown()
		{
			foreach (IAchievement achievement in achievements)
			{
				achievement.Shutdown(eventbus);
			}
			achievements.Clear();
			return ;
		}

		public void AddAchievement(IAchievement achievement)
		{
			if (achievements.Find(ac => ac.GetType() == achievement.GetType()) != null)
				return ;
			achievements.Add(achievement);
			achievement.Initialize(eventbus);
			return ;
		}

		public void Subscribe<GType01>(Action<GType01> handler) where GType01 : IEvent
		{
			eventbus.Subscribe(handler);
			return;
		}

		public void Unsubscribe<GType01>(Action<GType01> handler) where GType01 : IEvent
		{
			eventbus.Unsubscribe(handler);
			return;
		}

		public void Publish<GType01>(GType01 evt) where GType01 : IEvent
		{
			eventbus.Publish(evt);
			return;
		}
	}
}
