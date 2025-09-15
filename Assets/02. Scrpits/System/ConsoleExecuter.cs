using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{
	public class ConsoleExecuter : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("콘솔 UI")]
		private GameObject consoleUI;

		[SerializeField]
		[Tooltip("콘솔 입력")]
		private TMP_InputField consoleInput;

		[SerializeField]
		[Tooltip("콘솔 출력")]
		private TMP_Text consoleOutput;

		private Dictionary<string, IConsoleCommand> commands = new Dictionary<string, IConsoleCommand>();

		private void Start()
		{
			RegisterCommand();
			consoleUI.SetActive(false);
			return ;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.BackQuote) )
			{
				consoleUI.SetActive(!consoleUI.activeSelf);
				consoleOutput.text = "Console Result : \n";
			}
			return ;
		}

		/// <summary>
		/// 콘솔 명령어를 작성하면 실행하고 결과를 표시한다.
		/// </summary>
		public void OnSubmitCommand()
		{
			string input = consoleInput.text.Trim().ToLower();
			string[] args = input.Split(' ');

			consoleInput.text = "";
			ExecuteCommand(args);
			return;
		}

		/// <summary>
		/// 명령어를 딕셔너리에 등록한다.
		/// </summary>
		private void RegisterCommand()
		{
			Type commandType = typeof(IConsoleCommand);
			IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
										.Where(type => type.GetInterfaces().Contains(commandType) && !type.IsAbstract);

			foreach (Type type in types)
			{
				IConsoleCommand command = Activator.CreateInstance(type) as IConsoleCommand;

				if (command != null && !commands.ContainsKey(command.CommandName.ToLower()))
				{
					commands.Add(command.CommandName.ToLower(), command);
				}
			}
			return;
		}

		/// <summary>
		/// 콘솔 명령어를 실행한다.
		/// </summary>
		/// <param name="args">명령어의 인자들</param>
		private void ExecuteCommand(string[] args)
		{
			if (commands.ContainsKey(args[0]))
				consoleOutput.text += commands[args[0]].Execute(args) + "\n";
			else
				consoleOutput.text += $"Unknown command: {args[0]}\n";
			return;
		}
	}
}

