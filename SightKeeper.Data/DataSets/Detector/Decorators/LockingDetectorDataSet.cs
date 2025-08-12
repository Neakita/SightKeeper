using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class LockingDetectorDataSet(StorableDetectorDataSet inner, Lock editingLock) : StorableDetectorDataSet
{
	public string Name
	{
		get => inner.Name;
		set
		{
			lock (editingLock)
				inner.Name = value;
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			lock (editingLock)
				inner.Description = value;
		}
	}

	public StorableTagsOwner<StorableTag> TagsLibrary { get; } =
		new LockingTagsLibrary<StorableTag>(inner.TagsLibrary, editingLock);

	public StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary { get; } =
		new LockingAssetsLibrary<StorableItemsAsset<StorableDetectorItem>>(inner.AssetsLibrary, editingLock);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new LockingWeightsLibrary(inner.WeightsLibrary, editingLock);

	public StorableDetectorDataSet Innermost => inner.Innermost;
}