using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DTagsLibrary : TagsLibrary<Poser3DTag>
{
	public override Poser3DDataSet DataSet { get; }

	public Poser3DTag CreateTag(string name)
	{
		Poser3DTag tag = new(name, this);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(Poser3DTag tag)
	{
		Guard.IsEmpty(tag.Items);
		base.DeleteTag(tag);
	}

	internal Poser3DTagsLibrary(Poser3DDataSet dataSet)
	{
		DataSet = dataSet;
	}
}