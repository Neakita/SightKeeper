using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DAssetsLibrary : AssetsLibrary<Poser2DAsset>
{
	public override Poser2DDataSet DataSet { get; }

	public Poser2DAssetsLibrary(Poser2DDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected override Poser2DAsset CreateAsset(Screenshot<Poser2DAsset> screenshot)
	{
		return new Poser2DAsset(screenshot, this);
	}

	public override void DeleteAsset(Poser2DAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
		foreach (var item in asset.Items)
			item.Tag.RemoveItem(item);
	}
}