using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAssetsLibrary : AssetsLibrary<DetectorAsset>
{
	public override DetectorDataSet DataSet { get; }

	public DetectorAssetsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected override DetectorAsset CreateAsset(Screenshot<DetectorAsset> screenshot)
	{
		return new DetectorAsset(screenshot, this);
	}

	public override void DeleteAsset(DetectorAsset asset)
	{
		base.DeleteAsset(asset);
		foreach (var item in asset.Items)
			item.Tag.RemoveItem(item);
	}
}