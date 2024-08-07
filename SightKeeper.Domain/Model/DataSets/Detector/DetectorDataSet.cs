namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet
{
	public override TagsLibrary<DetectorTag> Tags { get; }
	public override AssetScreenshotsLibrary<DetectorAsset> Screenshots { get; }
	public override DetectorAssetsLibrary Assets { get; }
	public override WeightsLibrary<DetectorTag> Weights { get; }

	public DetectorDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new TagsLibrary<DetectorTag>(this);
		Screenshots = new AssetScreenshotsLibrary<DetectorAsset>(this);
		Assets = new DetectorAssetsLibrary(this);
		Weights = new WeightsLibrary<DetectorTag>(this);
	}
}