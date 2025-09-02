using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class DomainPoser3DDataSet : Poser3DDataSet
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
	public AssetsOwner<PoserAsset<DomainPoser3DItem>> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }

	public DomainPoser3DDataSet(Poser3DDataSet inner)
	{
		_inner = inner;
		TagsLibrary = new DomainTagsLibrary<PoserTag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}

	private readonly Poser3DDataSet _inner;
}