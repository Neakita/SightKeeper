using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class DomainPoserDataSet : PoserDataSet
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

	public TagsOwner<PoserTag> TagsLibrary { get; }
	public AssetsOwner<PoserAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }

	public DomainPoserDataSet(PoserDataSet inner)
	{
		_inner = inner;
		TagsLibrary = new DomainTagsLibrary<PoserTag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}

	private readonly PoserDataSet _inner;
}