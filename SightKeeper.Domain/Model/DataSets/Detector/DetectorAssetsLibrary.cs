namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAssetsLibrary : AssetsLibrary<DetectorAsset>
{
	public DetectorAsset MakeAsset(Screenshot screenshot)
	{
		DetectorAsset asset = new(screenshot);
		AddAsset(asset);
		return asset;
	}

	internal DetectorAssetsLibrary()
	{
	}
}