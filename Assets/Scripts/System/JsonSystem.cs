using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEditor;

public static class JsonSystem
{
	private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.Auto,
	};

	/// <summary>
	/// 객체 또는 값을 json으로 저장한다. 이때, Base64로 난독화할 수 있다.
	/// </summary>
	/// <typeparam name="GType1">저장하려는 대상의 Type</typeparam>
	/// <param name="data">저장하려는 대상</param>
	/// <param name="path">저장하려는 파일 이름</param>
	public static void SaveToJson<GType1>(GType1 data, string path, bool isBase64 = true)
	{
		string json;

		if (data == null)
			throw (new ArgumentNullException("data", "data cannot be null!"));
		if (path == "")
			throw (new ArgumentException("json fileName cannot be empty!", "path"));
		path = GetJsonPath(path);
		json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
		if (isBase64)
			json = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
		File.WriteAllText(path, json);
		return ;
	}

	/// <summary>
	/// 객체 또는 값을 json에서 불러온다. 이때, Base64로 난독화된 상태인지 확인해야 한다.
	/// </summary>
	/// <typeparam name="GType1">불러오려는 대상의 Type</typeparam>
	/// <param name="path">불러오려는 파일 이름</param>
	/// <returns>불러오는데 성공한 객체 또는 실패할 시 default</returns>
	public static GType1 LoadFromJson<GType1>(string path, bool isBase64 = true)
	{
		string json;

		if (path == "")
			throw (new ArgumentException("json fileName cannot be empty!", "path"));
		path = GetJsonPath(path);
		if (!File.Exists(path))
		{
			Debug.LogWarning($"There are no file! : {path}");
			return (default);
		}
		json = File.ReadAllText(path);
		if (isBase64)
			json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(json));
		return ((GType1)JsonConvert.DeserializeObject(json, typeof(GType1), settings));
	}

	public static bool HasJsonFile(string path, bool isDataPath = false)
	{
		if (!isDataPath)
		{
			path = GetJsonPath(path);
		}
		return (File.Exists(path));
	}

	public static string GetJsonPath(string fileName)
	{
		string result;

#if UNITY_EDITOR
		result = Path.Combine(Application.persistentDataPath, fileName + " (Editor).json");
#else
		result = Path.Combine(Application.persistentDataPath, fileName + ".json");
#endif
		return (result);
	}

#if UNITY_EDITOR
	[MenuItem("Tools/Open Save Folder")]
	public static void OpenPersistentDataPath()
	{
		EditorUtility.RevealInFinder(Application.persistentDataPath);
		return ;
	}
#endif

}