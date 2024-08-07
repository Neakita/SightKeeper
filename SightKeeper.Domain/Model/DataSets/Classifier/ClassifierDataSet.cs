namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet
{
	public override ClassifierTagsLibrary Tags { get; }
	public override ClassifierScreenshotsLibrary Screenshots { get; }
	public override ClassifierAssetsLibrary Assets { get; }
	public override WeightsLibrary<ClassifierTag> Weights { get; }

	public ClassifierDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new ClassifierTagsLibrary(this);
		Screenshots = new ClassifierScreenshotsLibrary(this);
		Assets = new ClassifierAssetsLibrary(this);
		Weights = new WeightsLibrary<ClassifierTag>(this);
	}
}