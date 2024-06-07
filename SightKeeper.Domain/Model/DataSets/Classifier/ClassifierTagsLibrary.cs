using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTagsLibrary : TagsLibrary<Tag>
{
	public ClassifierDataSet DataSet { get; }

	public Tag CreateTag(string name, uint color)
	{
		Tag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(Tag tag)
	{
		bool isTagInUse = DataSet.Assets.Any(asset => asset.Tag == tag);
		Guard.IsFalse(isTagInUse);
		base.DeleteTag(tag);
	}

	internal ClassifierTagsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}
}