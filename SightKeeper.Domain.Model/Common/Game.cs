using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class Game : Entity
{
	public string Title { get; set; }
	public string ProcessName { get; private set; }
	
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}
	
	private Game(int id, string title, string processName) : base(id)
	{
		Title = title;
		ProcessName = processName;
	}
}