using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class StorableTagFactory(ChangeListener changeListener, Lock editingLock) : TagFactory<Tag>
{
	public TagsContainer<Tag>? TagsOwner { get; set; }

	public Tag CreateTag(string name)
	{
		Guard.IsNotNull(TagsOwner);
		var inMemoryTag = new InMemoryTag
		{
			Name = name,
			Owner = TagsOwner
		};
		return inMemoryTag
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithIndex()
			.WithEditableUsers()
			.WithNotifications();
	}
}