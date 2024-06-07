using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTagsLibrary : TagsLibrary<ClassifierTag>
{
	public ClassifierDataSet DataSet { get; }

	public ClassifierTag CreateTag(string name, uint color)
	{
		ClassifierTag tag = new(name, color, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(ClassifierTag tag)
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