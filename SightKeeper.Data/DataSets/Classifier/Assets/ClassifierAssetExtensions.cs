using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal static class ClassifierAssetExtensions
{
	public static StorableClassifierAsset WithTagUsersTracking(this StorableClassifierAsset asset)
	{
		return new TagUsersTrackingClassifierAsset(asset);
	}

	public static StorableClassifierAsset WithNotifications(this StorableClassifierAsset asset)
	{
		return new NotifyingClassifierAsset(asset);
	}

	public static StorableClassifierAsset WithTracking(this StorableClassifierAsset asset, ChangeListener changeListener)
	{
		return new TrackingClassifierAsset(asset, changeListener);
	}

	public static StorableClassifierAsset WithLocking(this StorableClassifierAsset asset, Lock editingLock)
	{
		return new LockingClassifierAsset(asset, editingLock);
	}
}