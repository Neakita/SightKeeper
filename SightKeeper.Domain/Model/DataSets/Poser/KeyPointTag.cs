using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class KeyPointTag<TTag> : Tag where TTag : Tag
{
	public abstract TTag PoserTag { get; }
	public override DataSet DataSet => PoserTag.DataSet;

	protected KeyPointTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}