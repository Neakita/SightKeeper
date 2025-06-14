using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DomainDetectorDataSet : DetectorDataSet
{
	public string Name
	{
		get => _inner.Name;
		set => _inner.Name = value;
	}

	public string Description
	{
		get => _inner.Description;
		set => _inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; }
	public AssetsOwner<DetectorAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }

	public DomainDetectorDataSet(DetectorDataSet inner)
	{
		_inner = inner;
		TagsLibrary = new DomainTagsLibrary<Tag>(inner.TagsLibrary);
		AssetsLibrary = new DomainAssetsLibrary<DetectorAsset>(inner.AssetsLibrary);
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}

	private readonly DetectorDataSet _inner;
}