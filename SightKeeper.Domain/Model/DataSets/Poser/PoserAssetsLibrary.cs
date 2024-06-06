namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserAssetsLibrary : AssetsLibrary<PoserAsset>
{
	public PoserAsset MakeAsset(Screenshot screenshot)
	{
		PoserAsset asset = new(screenshot);
		AddAsset(asset);
		return asset;
	}
}