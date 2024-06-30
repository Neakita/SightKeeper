using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserAssetsLibrary : AssetsLibrary<PoserAsset>
{
	public override PoserDataSet DataSet { get; }

	public PoserAsset MakeAsset(PoserScreenshot screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		PoserAsset asset = new(screenshot, this);
		screenshot.SetAsset(asset);
		AddAsset(asset);
		return asset;
	}

	public override void DeleteAsset(PoserAsset asset)
	{
		base.DeleteAsset(asset);
		asset.Screenshot.SetAsset(null);
	}

	internal PoserAssetsLibrary(PoserDataSet dataSet)
	{
		DataSet = dataSet;
	}
}