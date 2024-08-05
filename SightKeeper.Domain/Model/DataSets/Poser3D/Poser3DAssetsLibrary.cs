using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DAssetsLibrary : AssetsLibrary<Poser3DAsset>
{
	public override Poser3DDataSet DataSet { get; }

	public Poser3DAsset MakeAsset(Poser3DScreenshot screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		Poser3DAsset asset = new(screenshot, this);
		screenshot.SetAsset(asset);
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(Poser3DAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
	}

	internal Poser3DAssetsLibrary(Poser3DDataSet dataSet)
	{
		DataSet = dataSet;
	}
}