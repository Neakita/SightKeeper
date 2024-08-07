namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet
{
	public override TagsLibrary<ClassifierTag> Tags { get; }
	public override AssetScreenshotsLibrary<ClassifierAsset> Screenshots { get; }
	public override AssetsLibrary<ClassifierAsset> Assets { get; }
	public override WeightsLibrary<ClassifierTag> Weights { get; }

	public ClassifierDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new TagsLibrary<ClassifierTag>(this);
		Screenshots = new AssetScreenshotsLibrary<ClassifierAsset>(this);
		Assets = new AssetsLibrary<ClassifierAsset>(this);
		Weights = new WeightsLibrary<ClassifierTag>(this);
	}
}