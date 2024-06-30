namespace SightKeeper.Domain.Model.DataSets;

public abstract class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;
	// https://github.com/dotnet/csharplang/discussions/6291
	// public abstract Asset? Asset { get; }
	public abstract ScreenshotsLibrary Library { get; }
}