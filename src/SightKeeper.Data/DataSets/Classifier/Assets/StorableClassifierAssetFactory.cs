using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal sealed class StorableClassifierAssetFactory(
	ChangeListener changeListener,
	Lock editingLock)
	: AssetFactory<ClassifierAsset>, PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>>
{
	public void Initialize(DataSet<Tag, ReadOnlyAsset> wrapped)
	{
		_tagsOwner = wrapped.TagsLibrary;
	}

	public ClassifierAsset CreateAsset(ManagedImage image)
	{
		Guard.IsNotNull(_tagsOwner);
		var wrapped = new InMemoryClassifierAsset(image, _tagsOwner.Tags[0])
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithTagUsersTracking(out var initializeUsersTracking)
			.WithNotifications();
		initializeUsersTracking(wrapped);
		return wrapped;
	}

	private TagsContainer<Tag>? _tagsOwner;
}