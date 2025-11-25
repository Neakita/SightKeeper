using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class StorableTagFactory(
	ChangeListener changeListener,
	Lock editingLock)
	: TagFactory<Tag>, PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>>
{
	public void Initialize(DataSet<Tag, ReadOnlyAsset> wrapped)
	{
		_tagsOwner = wrapped.TagsLibrary;
	}

	public void Initialize(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public Tag CreateTag(string name)
	{
		Guard.IsNotNull(_tagsOwner);
		var inMemoryTag = new InMemoryTag
		{
			Name = name,
			Owner = _tagsOwner
		};
		return inMemoryTag
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithIndex()
			.WithEditableUsers()
			.WithNotifications();
	}

	private TagsContainer<Tag>? _tagsOwner;
}