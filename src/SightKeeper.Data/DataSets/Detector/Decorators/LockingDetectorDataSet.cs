using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class LockingDetectorDataSet(DetectorDataSet inner, Lock editingLock) : DetectorDataSet, Decorator<DetectorDataSet>
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

	public TagsOwner<Tag> TagsLibrary { get; } =
		new LockingTagsLibrary<Tag>(inner.TagsLibrary, editingLock);

	public AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary { get; } =
		new LockingAssetsLibrary<ItemsAsset<DetectorItem>>(inner.AssetsLibrary, editingLock);

	public WeightsLibrary WeightsLibrary { get; } =
		new LockingWeightsLibrary(inner.WeightsLibrary, editingLock);

	public DetectorDataSet Inner => inner;
}