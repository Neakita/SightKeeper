namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorItem
{
	public DetectorTag Tag { get; set; }
	public Bounding Bounding { get; set; }
	public DetectorAsset Asset { get; }
	public DetectorDataSet DataSet => Asset.DataSet;
	
	internal DetectorItem(DetectorTag tag, Bounding bounding, DetectorAsset asset)
	{
		Tag = tag;
		Bounding = bounding;
		Asset = asset;
	}
}