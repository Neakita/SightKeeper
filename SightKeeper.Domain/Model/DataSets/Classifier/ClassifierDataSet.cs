namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet<ClassifierTag, ClassifierAsset>
{
	public override ClassifierAssetsLibrary AssetsLibrary { get; }

	public ClassifierDataSet()
	{
		AssetsLibrary = new ClassifierAssetsLibrary(this);
	}
}