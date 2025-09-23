using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ARegistry<GType1> : ScriptableObject where GType1 : ScriptableObject
{
	[SerializeField]
	private GType1[] all;
	private static Dictionary<string, GType1> _dict;

	private static ARegistry<GType1> _instance;
	public static ARegistry<GType1> Instance
	{
		get
		{
			if (_instance == null)
				_instance = Resources.Load<ARegistry<GType1>>("Registry/" + typeof(GType1).ToString());
			return (_instance);
		}
	}


	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitOnLoad()
	{
		Init();
		return;
	}

	private static void Init()
	{
		if (_dict != null)
			return;

		if (Instance == null)
		{
			Debug.LogError($"[Registry {typeof(GType1).ToString()}] Resources.Load failed. Make sure a Registry asset exists at 'Resources/Registry/{typeof(GType1).ToString()}'.");
			return;
		}
		if (Instance.all == null || Instance.all.Length == 0)
		{
			Debug.LogWarning($"[RegistryElement {typeof(GType1).ToString()}] 'all' is null or empty in the registry asset.");
			return;
		}
		_dict = new Dictionary<string, GType1>();
		for (int i = 0; i < Instance.all.Length; i++)
		{
			GType1 element = Instance.all[i];

			if (element == null)
			{
				Debug.LogWarning($"[RegistryElement {typeof(GType1).ToString()}] all at index {i} is null, skipping.");
				continue;
			}
			if (string.IsNullOrEmpty(element.name))
			{
				Debug.LogWarning($"[RegistryElement {typeof(GType1).ToString()}] all at index {i} has empty name, skipping.");
				continue;
			}
			if (_dict.ContainsKey(element.name))
				Debug.LogWarning($"[RegistryElement {typeof(GType1).ToString()}] duplicate SO name '{element.name}', overwriting previous entry.");

			_dict[element.name] = element;
		}
		return;
	}

	public static bool TryGet(string name, out GType1 enumObj)
	{
		if (_dict == null)
			Init();
		if (_dict == null)
		{
			enumObj = null;
			return (false);
		}
		return (_dict.TryGetValue(name, out enumObj));
	}

	public static GType1 Get(string name)
	{
		if (TryGet(name, out var enumObj))
			return (enumObj);
		Debug.LogError($"[Registry {typeof(GType1).ToString()}] No SO with name '{name}' found in registry.");
		return (null);
	}
}