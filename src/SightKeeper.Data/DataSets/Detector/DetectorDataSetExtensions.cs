using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Detector.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector;

internal static class DetectorDataSetExtensions
{
	public static DetectorDataSet WithTracking(this DetectorDataSet set, ChangeListener listener)
	{
		return new TrackableDetectorDataSet(set, listener);
	}

	public static DetectorDataSet WithLocking(this DetectorDataSet set, Lock editingLock)
	{
		return new LockingDetectorDataSet(set, editingLock);
	}

	public static DetectorDataSet WithWeightsDataRemoving(this DetectorDataSet set)
	{
		return new OverrideLibrariesDetectorDataSet(set)
		{
			WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static DetectorDataSet WithObservableLibraries(this DetectorDataSet set)
	{
		return new OverrideLibrariesDetectorDataSet(set)
		{
			TagsLibrary = new ObservableTagsLibrary<Tag>(set.TagsLibrary),
			AssetsLibrary = new ObservableAssetsLibrary<ItemsAsset<DetectorItem>>(set.AssetsLibrary),
			WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
		};
	}

	public static DetectorDataSet WithDomainRules(this DetectorDataSet set)
	{
		return new DomainDetectorDataSet(set);
	}
}