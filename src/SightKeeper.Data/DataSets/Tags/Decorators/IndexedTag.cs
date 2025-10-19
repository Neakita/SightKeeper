using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags.Decorators;

internal sealed class IndexedTag(Tag inner) : Tag, TagIndexHolder, Decorator<Tag>
{
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public TagsContainer<Tag> Owner => inner.Owner;
	public byte Index { get; set; }
	public Tag Inner => inner;

	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public uint Color
	{
		get => inner.Color;
		set => inner.Color = value;
	}
}