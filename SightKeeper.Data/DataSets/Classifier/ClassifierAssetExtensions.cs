namespace SightKeeper.Data.DataSets.Classifier;

internal static class ClassifierAssetExtensions
{
	public static StorableClassifierAsset WithTagUsersTracking(this StorableClassifierAsset asset)
	{
		return new TagUsersTrackingClassifierAsset(asset);
	}
}