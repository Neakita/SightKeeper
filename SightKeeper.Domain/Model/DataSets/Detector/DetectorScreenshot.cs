namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public override DetectorAsset? Asset => _asset;
	public override DetectorScreenshotsLibrary Library { get; }
	public override DetectorDataSet DataSet => Library.DataSet;

	internal DetectorScreenshot(DetectorScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(DetectorAsset? asset)
	{
		_asset = asset;
	}

	protected internal override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private DetectorAsset? _asset;
}