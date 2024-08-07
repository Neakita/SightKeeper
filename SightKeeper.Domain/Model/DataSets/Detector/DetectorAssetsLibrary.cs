using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAssetsLibrary : AssetsLibrary<DetectorAsset>
{
	public override DetectorDataSet DataSet { get; }

	public DetectorAsset MakeAsset(Screenshot<DetectorAsset> screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		DetectorAsset asset = new(screenshot, this);
		screenshot.SetAsset(asset);
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(DetectorAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
	}

	internal DetectorAssetsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}
}