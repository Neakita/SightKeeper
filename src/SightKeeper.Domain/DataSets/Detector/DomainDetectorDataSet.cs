using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DomainDetectorDataSet : DetectorDataSet, Decorator<DetectorDataSet>
{
	public string Name
	{
		get => Inner.Name;
		set => Inner.Name = value;
	}

	public string Description
	{
		get => Inner.Description;
		set => Inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; }
	public AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
	public DetectorDataSet Inner { get; }

	public DomainDetectorDataSet(DetectorDataSet inner)
	{
		Inner = inner;
		TagsLibrary = new DomainTagsLibrary<Tag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}
}