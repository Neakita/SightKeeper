namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAssetsLibrary : AssetsLibrary<DetectorAsset>
{
	public DetectorDataSet DataSet { get; }

	public DetectorAsset MakeAsset(Screenshot screenshot)
	{
		DetectorAsset asset = new(screenshot, this);
		AddAsset(asset);
		return asset;
	}

	internal DetectorAssetsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}
}