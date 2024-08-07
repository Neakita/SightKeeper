using MemoryPack;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal partial class Game
{
	public Domain.Model.Game ToGame() => new(Title, ProcessName, ExecutablePath);

	public Game(ushort id, Domain.Model.Game game)
	{
		Id = id;
		Title = game.Title;
		ProcessName = game.ProcessName;
		ExecutablePath = game.ExecutablePath;
	}

	[MemoryPackConstructor]
	public Game(ushort id, string title, string processName, string executablePath)
	{
		Id = id;
		Title = title;
		ProcessName = processName;
		ExecutablePath = executablePath;
	}

	public ushort Id { get; init; }
	public string Title { get; init; }
	public string ProcessName { get; init; }
	public string ExecutablePath { get; init; }
}