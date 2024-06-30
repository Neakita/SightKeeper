using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAssetsLibrary : AssetsLibrary<DetectorAsset>
{
	public override DetectorDataSet DataSet { get; }

	public DetectorAsset MakeAsset(DetectorScreenshot screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		DetectorAsset asset = new(screenshot, this);
		screenshot.Asset = asset;
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(DetectorAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.Asset = null;
	}

	internal DetectorAssetsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}
}