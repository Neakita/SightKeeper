using FlakeId;

namespace SightKeeper.Domain.Model.Common;

public sealed class Game
{
	public Id Id { get; private set; }
	public string Title { get; set; }
	public string ProcessName { get; set; }
	
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}

	public override string ToString() => Title;
}