using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
public partial class SerializableGame
{
	public Game ToGame() => new(Title, ProcessName, ExecutablePath);

	public SerializableGame(ushort id, Game game)
	{
		Id = id;
		Title = game.Title;
		ProcessName = game.ProcessName;
		ExecutablePath = game.ExecutablePath;
	}

	[MemoryPackConstructor]
	public SerializableGame(ushort id, string title, string processName, string executablePath)
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