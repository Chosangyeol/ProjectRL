using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Element Registry", menuName = "SO/Registry - Element")]
public class RegistryElement : ScriptableObject
{
	[SerializeField]
	private ElementType[] all;
	private static Dictionary<string, ElementType> _dict;

	private static RegistryElement _instance;
	public static RegistryElement Instance
	{
		get
		{
			if (_instance == null)
				_instance = Resources.Load<RegistryElement>("Registry/Element");
			return (_instance);
		}
	}


	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		_dict = new Dictionary<string, ElementType>();
		for (int i = 0; i < Instance.all.Length; i++)
		{
			ElementType element = Instance.all[i];

			_dict[element.name] = element;
		}
	}

	public static ElementType Get(string name) => _dict[name];
}