using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class StorableTagFactory(ChangeListener changeListener, Lock editingLock) : TagFactory<StorableTag>
{
	public TagsContainer<Tag>? TagsOwner { get; set; }

	public StorableTag CreateTag(string name)
	{
		Guard.IsNotNull(TagsOwner);
		var inMemoryTag = new InMemoryTag
		{
			Name = name,
			Owner = TagsOwner
		};
		return inMemoryTag
			.WithTracking(changeListener)
			.WithLocking(editingLock);
	}
}