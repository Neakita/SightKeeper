namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorItem
{
	public DetectorTag Tag { get; set; }
	public Bounding Bounding { get; set; }
	
	internal DetectorItem(DetectorTag tag, Bounding bounding)
	{
		Tag = tag;
		Bounding = bounding;
	}
}