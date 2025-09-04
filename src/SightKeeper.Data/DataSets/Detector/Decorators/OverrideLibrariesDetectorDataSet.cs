using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class OverrideLibrariesDetectorDataSet(DetectorDataSet inner) : DetectorDataSet, Decorator<DetectorDataSet>
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; init; } = inner.TagsLibrary;
	public AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary { get; init; } = inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary { get; init; } = inner.WeightsLibrary;
	public DetectorDataSet Inner => inner;
}