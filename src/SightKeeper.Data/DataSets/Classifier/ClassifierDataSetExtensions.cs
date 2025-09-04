using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Classifier.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal static class ClassifierDataSetExtensions
{
	public static ClassifierDataSet WithTracking(this ClassifierDataSet set, ChangeListener listener)
	{
		return new TrackableClassifierDataSet(set, listener);
	}

	public static ClassifierDataSet WithLocking(this ClassifierDataSet set, Lock editingLock)
	{
		return new LockingClassifierDataSet(set, editingLock);
	}

	public static ClassifierDataSet WithDomainRules(this ClassifierDataSet set)
	{
		return new DomainClassifierDataSet(set);
	}

	public static ClassifierDataSet WithNotifications(this ClassifierDataSet set)
	{
		return new NotifyingClassifierDataSet(set);
	}

	public static ClassifierDataSet WithWeightsDataRemoving(this ClassifierDataSet set)
	{
		return new OverrideLibrariesClassifierDataSet(set)
		{
			WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static ClassifierDataSet WithObservableLibraries(this ClassifierDataSet set)
	{
		return new OverrideLibrariesClassifierDataSet(set)
		{
			TagsLibrary = new ObservableTagsLibrary<Tag>(set.TagsLibrary),
			AssetsLibrary = new ObservableAssetsLibrary<ClassifierAsset>(set.AssetsLibrary),
			WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static ClassifierDataSet WithTagUsersTracking(this ClassifierDataSet set)
	{
		return new OverrideLibrariesClassifierDataSet(set)
		{
			AssetsLibrary = new TagUsersTrackingClassifierAssetsLibrary(set.AssetsLibrary)
		};
	}
}