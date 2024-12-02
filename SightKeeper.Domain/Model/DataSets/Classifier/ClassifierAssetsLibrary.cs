using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAssetsLibrary : AssetsLibrary<ClassifierAsset>
{
	public override ClassifierDataSet DataSet { get; }

	public ClassifierAssetsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected override ClassifierAsset CreateAsset(Screenshot<ClassifierAsset> screenshot)
	{
		return new ClassifierAsset(screenshot, DataSet.TagsLibrary.Tags.First(), this);
	}

	public override void DeleteAsset(ClassifierAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
		asset.Tag.RemoveAsset(asset);
	}
}