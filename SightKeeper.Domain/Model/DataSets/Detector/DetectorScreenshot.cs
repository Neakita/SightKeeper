namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public override DetectorAsset? Asset => _asset;
	public override DetectorScreenshotsLibrary Library { get; }

	internal DetectorScreenshot(DetectorScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(DetectorAsset? asset)
	{
		_asset = asset;
	}

	private DetectorAsset? _asset;
}