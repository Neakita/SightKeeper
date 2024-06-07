using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserTagsLibrary : TagsLibrary<PoserTag>
{
	public PoserDataSet DataSet { get; }

	public PoserTag CreateTag(string name, uint color)
	{
		PoserTag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(PoserTag tag)
	{
		bool isTagInUse = DataSet.Assets.SelectMany(asset => asset.Items).Any(item => item.Tag == tag);
		Guard.IsFalse(isTagInUse);
		base.DeleteTag(tag);
	}

	internal PoserTagsLibrary(PoserDataSet dataSet)
	{
		DataSet = dataSet;
	}
}