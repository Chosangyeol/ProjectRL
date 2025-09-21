namespace Console
{
	public interface IConsoleCommand
	{
		//커맨드의 이름. 내부적으로 소문자로 처리되며, 중복과 공백을 허용하지 않는다.
		string CommandName { get; }

		/// <summary>
		/// 명령어를 실행한다.
		/// </summary>
		/// <param name="args">명령어의 인자</param>
		/// <returns>명령 실행 결과 문구</returns>
		string Execute(string[] args);
	}
}
