using SightKeeper.Backend.Data.Members.Abstract;

namespace SightKeeper.Backend.Data.Members;

public sealed class Game
{
	public Guid Id { get; set; }
	public string Title { get; set; }
	public string ProcessName { get; set; }

	public List<Model> Models { get; set; } = new();

	public Game() : this(string.Empty, string.Empty) { }

	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}
}
