using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public class ClassifierAssetsLibrary : AssetsLibrary<ClassifierAsset>
{
	public override ClassifierDataSet DataSet { get; }

	public ClassifierAsset MakeAsset(ClassifierScreenshot screenshot, ClassifierTag tag)
	{
		Guard.IsNull(screenshot.Asset);
		ClassifierAsset asset = new(screenshot, tag, this);
		tag.AddAsset(asset);
		screenshot.SetAsset(asset);
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(ClassifierAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
		asset.Tag.RemoveAsset(asset);
	}

	internal ClassifierAssetsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}
}