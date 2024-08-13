using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class PoserTag : ItemTag
{
	public abstract override IReadOnlyCollection<PoserItem> Items { get; }

	protected PoserTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}