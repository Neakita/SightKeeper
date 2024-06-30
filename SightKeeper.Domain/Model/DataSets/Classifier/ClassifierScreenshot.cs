namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierScreenshot : Screenshot
{
	public override ClassifierAsset? Asset => _asset;
	public override ClassifierScreenshotsLibrary Library { get; }

	internal ClassifierScreenshot(ClassifierScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(ClassifierAsset? asset)
	{
		_asset = asset;
	}

	private ClassifierAsset? _asset;
}