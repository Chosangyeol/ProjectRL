using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnitySubCore.Json;
using UnitySubCore.Singleton;

namespace Config
{
	public class ConfigUserInput : ASingleton<ConfigUserInput>, IConfig
	{
		public event Action ActionCallbackConfigChanged;

		private readonly string path = "Config/InputSetting";

		private SInputSetting input;
		public SInputSetting InputKey { get => input; private set => input = value; }

		private Dictionary<string, KeyCode> dict;
		private Dictionary<string, InputKeyAxe> axis;

		public ConfigUserInput()
		{
			LoadData();
			SaveData();
			return;
		}

		public bool GetKey(string key)
		{
			KeyCode code = GetKeyCode(key);

			return (Input.GetKey(code));
		}

		public bool GetKeyDown(string key)
		{
			KeyCode code = GetKeyCode(key);

			return (Input.GetKeyDown(code));
		}

		public bool GetKeyUp(string key)
		{
			KeyCode code = GetKeyCode(key);

			return (Input.GetKeyUp(code));
		}

		public float GetAxis(string key, float deltaTime)
		{
			if (axis.TryGetValue(key, out InputKeyAxe axe))
				return (axe.GetAxis(deltaTime));
			throw (new ArgumentException($"{key} is not correct key"));
		}

		public KeyCode GetKeyCode(string key)
		{
			if (dict.TryGetValue(key, out KeyCode result))
				return (result);
			throw (new ArgumentException($"{key} is not correct key"));
		}

		private void SetDict()
		{
			FieldInfo[] array = typeof(SInputSetting).GetFields(BindingFlags.Public | BindingFlags.Instance);

			dict = new Dictionary<string, KeyCode>();
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo field = array[i];

				if (field.FieldType != typeof(KeyCode))
					continue ;
				dict[field.Name] = (KeyCode)(field.GetValue(InputKey));
			}
			// ==========
			axis = new Dictionary<string, InputKeyAxe>();
			
			axis["Horizontal"] = new InputKeyAxe();
			axis["Horizontal"].InitKeyCode(InputKey.keyMoveRight, InputKey.keyMoveLeft);
			axis["Horizontal"].InitField(0.001f, 3f, 3f);
			axis["Vertical"] = new InputKeyAxe();
			axis["Vertical"].InitKeyCode(InputKey.keyMoveFront, InputKey.keyMoveBack);
			axis["Vertical"].InitField(0.001f, 3f, 3f);
			return ;
		}

		public void LoadData()
		{
			InputKey = SCJson.LoadFromJson<SInputSetting>(path, true);

			input.Init(ref input);
			SetDict();
			return ;
		}

		public void SaveData()
		{
			SCJson.SaveToJson(InputKey, path, true);
			return;
		}

		public void OnChangeConfig()
		{
			SetDict();
			SaveData();
			ActionCallbackConfigChanged?.Invoke();
			return;
		}
	}
}
