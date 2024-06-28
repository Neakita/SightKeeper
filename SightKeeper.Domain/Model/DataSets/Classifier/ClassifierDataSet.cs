namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet
{
	public ClassifierTagsLibrary Tags { get; }
	public ClassifierScreenshotsLibrary Screenshots { get; }
	public ClassifierAssetsLibrary Assets { get; }
	public ClassifierWeightsLibrary Weights { get; }

	public ClassifierDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new ClassifierTagsLibrary(this);
		Screenshots = new ClassifierScreenshotsLibrary(this);
		Assets = new ClassifierAssetsLibrary(this);
		Weights = new ClassifierWeightsLibrary(this);
	}
}