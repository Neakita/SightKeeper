using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags;

internal sealed class StorablePoserTagFactory(ChangeListener changeListener, Lock editingLock) : TagFactory<PoserTag>
{
	public TagsContainer<PoserTag>? TagsOwner { get; set; }

	public PoserTag CreateTag(string name)
	{
		Guard.IsNotNull(TagsOwner);
		var keyPointTagFactory = new StorableTagFactory(changeListener, editingLock);
		var inMemoryTag = new InMemoryPoserTag(keyPointTagFactory)
		{
			Name = name,
			Owner = TagsOwner
		};
		var wrappedTag = inMemoryTag
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithEditableUsers()
			.WithNotifications();
		keyPointTagFactory.TagsOwner = wrappedTag;
		return wrappedTag;
	}
}