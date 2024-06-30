using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTagsLibrary : TagsLibrary<DetectorTag>
{
	public override DetectorDataSet DataSet { get; }
	
	public DetectorTag CreateTag(string name)
	{
		DetectorTag tag = new(name, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(DetectorTag tag)
	{
		Guard.IsEmpty(tag.Items);
		base.DeleteTag(tag);
	}

	internal DetectorTagsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}
}