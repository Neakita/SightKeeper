using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

internal static class ClassifierDataSetExtensions
{
	public static StorableClassifierDataSet WithTracking(this StorableClassifierDataSet set, ChangeListener listener)
	{
		return new TrackableClassifierDataSet(set, listener);
	}

	public static StorableClassifierDataSet WithLocking(this StorableClassifierDataSet set, Lock editingLock)
	{
		return new LockingClassifierDataSet(set, editingLock);
	}

	public static StorableClassifierDataSet WithDomainRules(this StorableClassifierDataSet set)
	{
		return new StorableClassifierDataSetExtension(new DomainClassifierDataSet(set), set);
	}

	public static StorableClassifierDataSet WithNotifications(this StorableClassifierDataSet set)
	{
		return new NotifyingClassifierDataSet(set);
	}

	public static StorableClassifierDataSet WithWeightsDataRemoving(this StorableClassifierDataSet set)
	{
		return new OverrideLibrariesClassifierDataSet(set)
		{
			WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static StorableClassifierDataSet WithObservableLibraries(this StorableClassifierDataSet set)
	{
		return new OverrideLibrariesClassifierDataSet(set)
		{
			TagsLibrary = new ObservableTagsLibrary<StorableTag>(set.TagsLibrary),
			AssetsLibrary = new ObservableAssetsLibrary<StorableClassifierAsset>(set.AssetsLibrary),
			WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static StorableClassifierDataSet WithTagUsersTracking(this StorableClassifierDataSet set)
	{
		return new OverrideLibrariesClassifierDataSet(set)
		{
			AssetsLibrary = new TagUsersTrackingClassifierAssetsLibrary(set.AssetsLibrary)
		};
	}
}