using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags.Decorators;

internal sealed class LockingTag(Tag inner, Lock editingLock): Tag, Decorator<Tag>
{
	public string Name
	{
		get => inner.Name;
		set
		{
			lock (editingLock)
				inner.Name = value;
		}
	}

	public uint Color
	{
		get => inner.Color;
		set
		{
			lock (editingLock)
				inner.Color = value;
		}
	}

	public TagsContainer<Tag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public Tag Inner => inner;
}