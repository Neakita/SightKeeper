using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal sealed class StorableClassifierAssetFactory(ChangeListener changeListener, Lock editingLock) : AssetFactory<StorableClassifierAsset>
{
	public TagsContainer<StorableTag>? TagsOwner { get; set; }

	public StorableClassifierAsset CreateAsset(StorableImage image)
	{
		Guard.IsNotNull(TagsOwner);
		var wrapped = new InMemoryClassifierAsset(image, TagsOwner.Tags[0])
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithTagUsersTracking(out var initializeUsersTracking)
			.WithNotifications();
		initializeUsersTracking(wrapped);
		return wrapped;
	}
}