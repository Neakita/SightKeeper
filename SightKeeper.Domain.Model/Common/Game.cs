namespace SightKeeper.Domain.Model.Common;

public sealed class Game
{
	public string Title { get; set; }
	public string ProcessName { get; set; }
	
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}

	public override string ToString() => Title;
}