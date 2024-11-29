using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class KeyPointTag : Tag
{
	public abstract PoserTag PoserTag { get; }
	public override DataSet DataSet => PoserTag.DataSet;
	public int Index { get; }

	protected KeyPointTag(string name, IEnumerable<Tag> siblings, int index) : base(name, siblings)
	{
		Index = index;
	}
}

public abstract class KeyPointTag<TTag> : KeyPointTag where TTag : PoserTag
{
	public abstract override TTag PoserTag { get; }
	public override DataSet DataSet => PoserTag.DataSet;
	public override bool IsInUse => PoserTag.Items.Count != 0;

	protected KeyPointTag(string name, IEnumerable<Tag> siblings, int index) : base(name, siblings, index)
	{
	}
}