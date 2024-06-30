using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserTagsLibrary : TagsLibrary<PoserTag>
{
	public override PoserDataSet DataSet { get; }

	public PoserTag CreateTag(string name)
	{
		PoserTag tag = new(name, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(PoserTag tag)
	{
		Guard.IsEmpty(tag.Items);
		base.DeleteTag(tag);
	}

	internal PoserTagsLibrary(PoserDataSet dataSet)
	{
		DataSet = dataSet;
	}
}