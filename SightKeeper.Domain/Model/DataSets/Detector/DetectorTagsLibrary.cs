using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTagsLibrary : TagsLibrary<Tag>
{
	public DetectorDataSet DataSet { get; }
	
	public Tag CreateTag(string name, uint color)
	{
		Tag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(Tag tag)
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