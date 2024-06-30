using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTagsLibrary : TagsLibrary<ClassifierTag>
{
	public override ClassifierDataSet DataSet { get; }

	public ClassifierTag CreateTag(string name)
	{
		ClassifierTag tag = new(name, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(ClassifierTag tag)
	{
		Guard.IsEmpty(tag.Assets);
		base.DeleteTag(tag);
	}

	internal ClassifierTagsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}
}