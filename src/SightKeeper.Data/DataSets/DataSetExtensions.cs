using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal static class DataSetExtensions
{
	public static DataSet<TTag, TAsset> WithTracking<TTag, TAsset>(this DataSet<TTag, TAsset> set, ChangeListener listener)
	{
		return new TrackableDataSet<TTag, TAsset>(set, listener);
	}

	public static DataSet<TTag, TAsset> WithLocking<TTag, TAsset>(this DataSet<TTag, TAsset> set, Lock editingLock)
	{
		return new LockingDataSet<TTag, TAsset>(set, editingLock);
	}

	public static DataSet<TTag, TAsset> WithDomainRules<TTag, TAsset>(this DataSet<TTag, TAsset> set) where TTag : Tag
	{
		var minimumTagsCount = set switch
		{
			DataSet<Tag, ClassifierAsset> => 2,
			_ => 1
		};
		return new DomainDataSet<TTag, TAsset>(set, minimumTagsCount);
	}

	public static DataSet<TTag, TAsset> WithNotifications<TTag, TAsset>(this DataSet<TTag, TAsset> set)
	{
		return new NotifyingDataSet<TTag, TAsset>(set);
	}

	public static DataSet<TTag, TAsset> WithWeightsDataRemoving<TTag, TAsset>(this DataSet<TTag, TAsset> set)
	{
		return new OverrideLibrariesDataSet<TTag, TAsset>(set)
		{
			WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static DataSet<TTag, TAsset> WithObservableLibraries<TTag, TAsset>(this DataSet<TTag, TAsset> set)
	{
		return new OverrideLibrariesDataSet<TTag, TAsset>(set)
		{
			TagsLibrary = new ObservableTagsLibrary<TTag>(set.TagsLibrary),
			AssetsLibrary = new ObservableAssetsLibrary<TAsset>(set.AssetsLibrary),
			WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static DataSet<TTag, TAsset> WithTagUsersTracking<TTag, TAsset>(this DataSet<TTag, TAsset> set)
	{
		return set switch
		{
			DataSet<Tag, ClassifierAsset> classifierSet => (DataSet<TTag, TAsset>)classifierSet.WithTagUsersTracking(),
			_ => set
		};
	}

	private static DataSet<Tag, ClassifierAsset> WithTagUsersTracking(this DataSet<Tag, ClassifierAsset> set)
	{
		return new OverrideLibrariesDataSet<Tag, ClassifierAsset>(set)
		{
			AssetsLibrary = new TagUsersTrackingClassifierAssetsLibrary(set.AssetsLibrary)
		};
	}

	public static DataSet<TTag, TAsset> WithSerialization<TTag, TAsset>(this DataSet<TTag, TAsset> set, ushort unionTag, TagsFormatter<TTag> tagsFormatter, AssetsFormatter<TAsset> assetsFormatter, WeightsFormatter weightsFormatter) where TTag : Tag
	{
		return new SerializableDataSet<TTag, TAsset>(set, unionTag, tagsFormatter, assetsFormatter, weightsFormatter);
	}

	public static DataSet<TTag, TAsset> WithIndexedTagTracking<TTag, TAsset>(this DataSet<TTag, TAsset> set) where TTag : notnull
	{
		return new OverrideLibrariesDataSet<TTag, TAsset>(set)
		{
			TagsLibrary = new IndexedTagTrackingTagsLibrary<TTag>(set.TagsLibrary)
		};
	}
}