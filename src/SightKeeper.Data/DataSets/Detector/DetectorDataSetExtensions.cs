using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Decorators;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Data.DataSets.Detector;

internal static class DetectorDataSetExtensions
{
	public static StorableDetectorDataSet WithTracking(this StorableDetectorDataSet set, ChangeListener listener)
	{
		return new TrackableDetectorDataSet(set, listener);
	}

	public static StorableDetectorDataSet WithLocking(this StorableDetectorDataSet set, Lock editingLock)
	{
		return new LockingDetectorDataSet(set, editingLock);
	}

	public static StorableDetectorDataSet WithWeightsDataRemoving(this StorableDetectorDataSet set)
	{
		return new OverrideLibrariesDetectorDataSet(set)
		{
			WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static StorableDetectorDataSet WithObservableLibraries(this StorableDetectorDataSet set)
	{
		return new OverrideLibrariesDetectorDataSet(set)
		{
			TagsLibrary = new ObservableTagsLibrary<StorableTag>(set.TagsLibrary),
			AssetsLibrary = new ObservableAssetsLibrary<StorableItemsAsset<StorableDetectorItem>>(set.AssetsLibrary),
			WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static StorableDetectorDataSet WithDomainRules(this StorableDetectorDataSet set)
	{
		return new StorableDetectorDataSetExtension(new DomainDetectorDataSet(set), set);
	}
}