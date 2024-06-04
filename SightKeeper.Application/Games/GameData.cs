namespace SightKeeper.Application.Games;

public interface GameData
{
	string Title { get; }
	string ProcessName { get; }
	string ExecutablePath { get; }
}