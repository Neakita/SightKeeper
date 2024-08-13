using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class KeyPointTag<TTag> : Tag where TTag : PoserTag
{
	public abstract TTag PoserTag { get; }
	public override DataSet DataSet => PoserTag.DataSet;
	public override bool IsInUse => PoserTag.Items.Count != 0;

	protected KeyPointTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}