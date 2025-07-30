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
}