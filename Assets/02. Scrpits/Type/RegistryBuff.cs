using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Registry", menuName = "SO/Registry - Buff")]
public class RegistryBuff : ScriptableObject
{
	[SerializeField]
	private BuffType[] all;
	private static Dictionary<string, BuffType> _dict;

	private static RegistryBuff _instance;
	public static RegistryBuff Instance
	{
		get
		{
			if (_instance == null)
				_instance = Resources.Load<RegistryBuff>("Registry/Buff");
			return (_instance);
		}
	}


	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		_dict = new Dictionary<string, BuffType>();
		for (int i = 0; i < Instance.all.Length; i++)
		{
			BuffType buff = Instance.all[i];

			_dict[buff.name] = buff;
		}

		return ;
	}

	public static BuffType Get(string name) => _dict[name];
}