using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Singleton<GType1> : MonoBehaviour where GType1 : MonoBehaviour
{
	private static GType1			_instance;

	private static readonly object	_locker = new object();
	private static bool				_isDestroyed = false;

	public static GType1 Instance
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return (null);
#endif
			if (_isDestroyed)
			{
				Debug.LogWarning($"파괴된 Singleton : {typeof(GType1).ToString()}에 접근 시도함!");
				return (null);
			}
			lock (_locker)
			{
				if (Singleton<GType1>._instance == null)
				{
					_instance = FindAnyObjectByType<GType1>();
					if (_instance == null)
					{
						GameObject singletonObject = new GameObject();

						singletonObject.AddComponent<GType1>();
						singletonObject.name = typeof(GType1).ToString() + " (Singleton)";
					}
				}
			}
			return (_instance);
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as GType1;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
		return ;
	}

	private void OnApplicationQuit()
	{
		_isDestroyed = true;
	}
}
