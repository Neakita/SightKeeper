using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class TrackableDetectorDataSet(StorableDetectorDataSet inner, ChangeListener listener) : StorableDetectorDataSet
{
	public string Name
	{
		get => inner.Name;
		set
		{
			inner.Name = value;
			listener.SetDataChanged();
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			inner.Description = value;
			listener.SetDataChanged();
		}
	}

	public StorableTagsOwner<StorableTag> TagsLibrary { get; } =
		new TrackableTagsLibrary<StorableTag>(inner.TagsLibrary, listener);

	public StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary { get; } =
		new TrackableAssetsLibrary<StorableItemsAsset<StorableDetectorItem>>(inner.AssetsLibrary, listener);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new TrackableWeightsLibrary(inner.WeightsLibrary, listener);

	public StorableDetectorDataSet Innermost => inner.Innermost;
}