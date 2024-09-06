using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Model;

/// <summary>
/// MemoryPackable version of <see cref="Game"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableGame
{
	public ushort Id { get; }
	public string Title { get; }
	public string ProcessName { get; }
	public string ExecutablePath { get; }

	public PackableGame(ushort id, string title, string processName, string executablePath)
	{
		Id = id;
		Title = title;
		ProcessName = processName;
		ExecutablePath = executablePath;
	}
}