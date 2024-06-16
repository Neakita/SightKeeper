using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public class ClassifierAssetsLibrary : AssetsLibrary<ClassifierAsset>
{
	public ClassifierDataSet DataSet { get; }

	public ClassifierAsset MakeAsset(Screenshot screenshot, ClassifierTag tag)
	{
		Guard.IsTrue(DataSet.Tags.Contains(tag));
		ClassifierAsset asset = new(screenshot, tag, this);
		AddAsset(asset);
		return asset;
	}

	internal ClassifierAssetsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}
}