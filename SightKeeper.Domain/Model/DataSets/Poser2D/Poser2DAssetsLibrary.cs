using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DAssetsLibrary : AssetsLibrary<Poser2DAsset>
{
	public override Poser2DDataSet DataSet { get; }

	public Poser2DAsset MakeAsset(Poser2DScreenshot screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		Poser2DAsset asset = new(screenshot, this);
		screenshot.SetAsset(asset);
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(Poser2DAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
	}

	internal Poser2DAssetsLibrary(Poser2DDataSet dataSet)
	{
		DataSet = dataSet;
	}
}