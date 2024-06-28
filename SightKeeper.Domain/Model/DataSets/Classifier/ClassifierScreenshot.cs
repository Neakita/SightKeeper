namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierScreenshot : Screenshot
{
	public ClassifierAsset? Asset { get; set; }
	public ClassifierScreenshotsLibrary Library { get; }

	internal ClassifierScreenshot(ClassifierScreenshotsLibrary library)
	{
		Library = library;
	}
}