namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierScreenshot : Screenshot
{
	public ClassifierAsset? Asset { get; internal set; }
	public override ClassifierScreenshotsLibrary Library { get; }

	internal ClassifierScreenshot(ClassifierScreenshotsLibrary library)
	{
		Library = library;
	}
}