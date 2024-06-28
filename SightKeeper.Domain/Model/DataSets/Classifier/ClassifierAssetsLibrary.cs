using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public class ClassifierAssetsLibrary : AssetsLibrary<ClassifierAsset>
{
	public ClassifierDataSet DataSet { get; }

	public ClassifierAsset MakeAsset(ClassifierScreenshot screenshot, ClassifierTag tag)
	{
		Guard.IsNull(screenshot.Asset);
		ClassifierAsset asset = new(screenshot, tag, this);
		screenshot.Asset = asset;
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(ClassifierAsset asset)
	{
		var screenshot = asset.Screenshot;
		base.DeleteAsset(asset);
		screenshot.Asset = null;
	}

	internal ClassifierAssetsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}
}