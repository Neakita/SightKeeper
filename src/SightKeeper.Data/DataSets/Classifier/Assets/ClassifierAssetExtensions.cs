using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal static class ClassifierAssetExtensions
{
	public static ClassifierAsset WithTagUsersTracking(this ClassifierAsset asset, out Action<TagUser> initializeUsersTracking)
	{
		var trackingAsset = new TagUsersTrackingClassifierAsset(asset);
		initializeUsersTracking = user => trackingAsset.TagUser = user;
		return trackingAsset;
	}

	public static ClassifierAsset WithNotifications(this ClassifierAsset asset)
	{
		return new NotifyingClassifierAsset(asset);
	}

	public static ClassifierAsset WithTracking(this ClassifierAsset asset, ChangeListener changeListener)
	{
		return new TrackingClassifierAsset(asset, changeListener);
	}

	public static ClassifierAsset WithLocking(this ClassifierAsset asset, Lock editingLock)
	{
		return new LockingClassifierAsset(asset, editingLock);
	}
}