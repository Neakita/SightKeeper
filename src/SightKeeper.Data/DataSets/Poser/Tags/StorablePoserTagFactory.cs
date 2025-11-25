using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags;

internal sealed class StorablePoserTagFactory(
	ChangeListener changeListener,
	Lock editingLock)
	: TagFactory<PoserTag>, PostWrappingInitializable<DataSet<PoserTag, ReadOnlyAsset>>
{
	public void Initialize(DataSet<PoserTag, ReadOnlyAsset> wrapped)
	{
		_tagsOwner = wrapped.TagsLibrary;
	}

	public PoserTag CreateTag(string name)
	{
		Guard.IsNotNull(_tagsOwner);
		var keyPointTagFactory = new StorableTagFactory(changeListener, editingLock);
		var inMemoryTag = new InMemoryPoserTag(keyPointTagFactory)
		{
			Name = name,
			Owner = _tagsOwner
		};
		var wrappedTag = inMemoryTag
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithIndexedTagTracking()
			.WithIndex()
			.WithEditableUsers()
			.WithNotifications();
		keyPointTagFactory.Initialize(wrappedTag);
		return wrappedTag;
	}

	private TagsContainer<PoserTag>? _tagsOwner;
}