namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet<DetectorTag, DetectorAsset>
{
	public override DetectorAssetsLibrary AssetsLibrary { get; }

	public DetectorDataSet()
	{
		AssetsLibrary = new DetectorAssetsLibrary(this);
	}
}