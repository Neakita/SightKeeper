using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DAssetsLibrary : AssetsLibrary<Poser3DAsset>
{
	public override Poser3DDataSet DataSet { get; }

	public Poser3DAssetsLibrary(Poser3DDataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected override Poser3DAsset CreateAsset(Screenshot<Poser3DAsset> screenshot)
	{
		return new Poser3DAsset(screenshot, this);
	}

	public override void DeleteAsset(Poser3DAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
		foreach (var item in asset.Items)
			item.Tag.RemoveItem(item);
	}
}