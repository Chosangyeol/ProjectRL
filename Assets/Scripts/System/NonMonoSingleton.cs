using System;
using UnityEngine;

public abstract class NonMonoSingleton<T> where T : class
{
	public static T Instance
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return (null);
#endif
			if (NonMonoSingleton<T>._instance == null)
			{
				NonMonoSingleton<T>._instance = Activator.CreateInstance<T>();
			}
			return NonMonoSingleton<T>._instance;
		}
	}

	// Token: 0x04000206 RID: 518
	protected static T _instance;
}
