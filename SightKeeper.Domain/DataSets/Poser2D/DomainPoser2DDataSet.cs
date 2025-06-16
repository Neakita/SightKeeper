using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class DomainPoser2DDataSet : Poser2DDataSet
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
	public AssetsOwner<PoserAsset<Poser2DItem>> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }

	public DomainPoser2DDataSet(Poser2DDataSet inner)
	{
		_inner = inner;
		TagsLibrary = new DomainTagsLibrary<PoserTag>(inner.TagsLibrary);
		AssetsLibrary = new DomainAssetsLibrary<PoserAsset<Poser2DItem>>(inner.AssetsLibrary);
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}

	private readonly Poser2DDataSet _inner;
}