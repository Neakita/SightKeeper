using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal static class DataSetExtensions
{
	public static DataSet<TAsset> WithTracking<TAsset>(this DataSet<TAsset> set, ChangeListener listener)
	{
		return new TrackableDataSet<TAsset>(set, listener);
	}

	public static DataSet<TAsset> WithLocking<TAsset>(this DataSet<TAsset> set, Lock editingLock)
	{
		return new LockingDataSet<TAsset>(set, editingLock);
	}

	public static DataSet<TAsset> WithDomainRules<TAsset>(this DataSet<TAsset> set)
	{
		var minimumTagsCount = set switch
		{
			DataSet<ClassifierAsset> => 2,
			_ => 1
		};
		return new DomainDataSet<TAsset>(set, minimumTagsCount);
	}

	public static DataSet<TAsset> WithNotifications<TAsset>(this DataSet<TAsset> set)
	{
		return new NotifyingDataSet<TAsset>(set);
	}

	public static DataSet<TAsset> WithWeightsDataRemoving<TAsset>(this DataSet<TAsset> set)
	{
		return new OverrideLibrariesDataSet<TAsset>(set)
		{
			WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static DataSet<TAsset> WithObservableLibraries<TAsset>(this DataSet<TAsset> set)
	{
		return new OverrideLibrariesDataSet<TAsset>(set)
		{
			TagsLibrary = new ObservableTagsLibrary<Tag>(set.TagsLibrary),
			AssetsLibrary = new ObservableAssetsLibrary<TAsset>(set.AssetsLibrary),
			WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static DataSet<TAsset> WithTagUsersTracking<TAsset>(this DataSet<TAsset> set)
	{
		return set switch
		{
			DataSet<ClassifierAsset> classifierSet => (DataSet<TAsset>)classifierSet.WithTagUsersTracking(),
			_ => set
		};
	}

	private static DataSet<ClassifierAsset> WithTagUsersTracking(this DataSet<ClassifierAsset> set)
	{
		return new OverrideLibrariesDataSet<ClassifierAsset>(set)
		{
			AssetsLibrary = new TagUsersTrackingClassifierAssetsLibrary(set.AssetsLibrary)
		};
	}
}