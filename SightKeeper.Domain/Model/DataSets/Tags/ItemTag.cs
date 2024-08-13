using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class ItemTag : Tag
{
	public abstract IReadOnlyCollection<AssetItem> Items { get; }
	public override bool IsInUse => Items.Count != 0;

	protected ItemTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}