namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierScreenshot : Screenshot
{
	public override ClassifierAsset? Asset => _asset;
	public override ClassifierScreenshotsLibrary Library { get; }
	public override ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierScreenshot(ClassifierScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(ClassifierAsset? asset)
	{
		_asset = asset;
	}

	protected internal override void DeleteFromLibrary()
	{
		Library.DeleteScreenshot(this);
	}

	private ClassifierAsset? _asset;
}