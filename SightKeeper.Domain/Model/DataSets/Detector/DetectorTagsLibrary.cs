using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTagsLibrary : TagsLibrary<DetectorTag>
{
	public DetectorDataSet DataSet { get; }
	
	public DetectorTag CreateTag(string name, uint color)
	{
		DetectorTag tag = new(name, color, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(DetectorTag tag)
	{
		bool isTagInUse = DataSet.Assets.SelectMany(asset => asset.Items).Any(item => item.Tag == tag);
		Guard.IsFalse(isTagInUse);
		base.DeleteTag(tag);
	}

	internal DetectorTagsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}
}