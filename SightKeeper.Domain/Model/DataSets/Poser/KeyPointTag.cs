using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class KeyPointTag : Tag
{
	public abstract Tag PoserTag { get; }
	public override DataSet DataSet => PoserTag.DataSet;

	protected KeyPointTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}

public abstract class KeyPointTag<TTag> : KeyPointTag where TTag : PoserTag
{
	public abstract override TTag PoserTag { get; }
	public override DataSet DataSet => PoserTag.DataSet;
	public override bool IsInUse => PoserTag.Items.Count != 0;

	protected KeyPointTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}