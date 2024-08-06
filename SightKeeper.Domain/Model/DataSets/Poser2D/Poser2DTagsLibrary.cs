using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DTagsLibrary : TagsLibrary<Poser2DTag>
{
	public override Poser2DDataSet DataSet { get; }

	public Poser2DTag CreateTag(string name)
	{
		Poser2DTag tag = new(name, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(Poser2DTag tag)
	{
		Guard.IsEmpty(tag.Items);
		base.DeleteTag(tag);
	}

	internal Poser2DTagsLibrary(Poser2DDataSet dataSet)
	{
		DataSet = dataSet;
	}
}