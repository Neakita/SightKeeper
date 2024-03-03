namespace SightKeeper.Domain.Model;

public sealed class Game : Entity
{
	public string Title { get; set; }
	public string ProcessName { get; set; }
	public string? ExecutablePath { get; set; }

	public Game(string title, string processName, string? executablePath)
	{
		Title = title;
		ProcessName = processName;
		ExecutablePath = executablePath;
	}

	public override string ToString() => Title;
}