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

internal sealed class TrackableDetectorDataSet(DetectorDataSet inner, ChangeListener listener) : DetectorDataSet, Decorator<DetectorDataSet>
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

	public TagsOwner<Tag> TagsLibrary { get; } =
		new TrackableTagsLibrary<Tag>(inner.TagsLibrary, listener);

	public AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary { get; } =
		new TrackableAssetsLibrary<ItemsAsset<DetectorItem>>(inner.AssetsLibrary, listener);

	public WeightsLibrary WeightsLibrary { get; } =
		new TrackableWeightsLibrary(inner.WeightsLibrary, listener);

	public DetectorDataSet Inner => inner;
}