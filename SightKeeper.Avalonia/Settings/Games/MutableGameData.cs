namespace SightKeeper.Avalonia.Settings.Games;

internal interface MutableGameData
{
	string Title { get; set; }
	string ProcessName { get; set; }
	string? ExecutablePath { get; set; }
}