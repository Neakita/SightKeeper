namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserScreenshot : Screenshot
{
	public PoserAsset? Asset { get; internal set; }
	public override PoserScreenshotsLibrary Library { get; }

	internal PoserScreenshot(PoserScreenshotsLibrary library)
	{
		Library = library;
	}
}