using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal sealed class StorableClassifierAssetFactory(ChangeListener changeListener, Lock editingLock) : AssetFactory<ClassifierAsset>
{
	public TagsContainer<Tag>? TagsOwner { get; set; }

	public ClassifierAsset CreateAsset(ManagedImage image)
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