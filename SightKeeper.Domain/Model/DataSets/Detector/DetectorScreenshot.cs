namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public DetectorAsset? Asset { get; set; }
	public DetectorScreenshotsLibrary Library { get; }

	public DetectorScreenshot(DetectorScreenshotsLibrary library)
	{
		Library = library;
	}
}