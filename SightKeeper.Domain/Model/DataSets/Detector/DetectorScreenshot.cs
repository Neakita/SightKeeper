namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public DetectorAsset? Asset { get; internal set; }
	public override DetectorScreenshotsLibrary Library { get; }

	internal DetectorScreenshot(DetectorScreenshotsLibrary library)
	{
		Library = library;
	}
}