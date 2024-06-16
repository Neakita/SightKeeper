namespace SightKeeper.Domain.Model.DataSets;

public sealed class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;
	public ScreenshotsLibrary Library { get; }

	internal Screenshot(ScreenshotsLibrary library)
	{
		Library = library;
	}
}