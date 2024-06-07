namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserAssetsLibrary : AssetsLibrary<PoserAsset>
{
	public PoserDataSet DataSet { get; }

	public PoserAsset MakeAsset(Screenshot screenshot)
	{
		PoserAsset asset = new(screenshot, this);
		AddAsset(asset);
		return asset;
	}

	internal PoserAssetsLibrary(PoserDataSet dataSet)
	{
		DataSet = dataSet;
	}
}