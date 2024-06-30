namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet
{
	public override DetectorTagsLibrary Tags { get; }
	public override DetectorScreenshotsLibrary Screenshots { get; }
	public override DetectorAssetsLibrary Assets { get; }
	public override DetectorWeightsLibrary Weights { get; }

	public DetectorDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new DetectorTagsLibrary(this);
		Screenshots = new DetectorScreenshotsLibrary(this);
		Assets = new DetectorAssetsLibrary(this);
		Weights = new DetectorWeightsLibrary(this);
	}
}