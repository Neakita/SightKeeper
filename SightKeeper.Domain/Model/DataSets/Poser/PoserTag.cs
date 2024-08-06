namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class PoserTag : Tag
{
	public abstract IReadOnlyCollection<PoserItem> Items { get; }

	protected PoserTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}