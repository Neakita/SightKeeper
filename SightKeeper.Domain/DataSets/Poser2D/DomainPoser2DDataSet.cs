using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class DomainPoser2DDataSet : Poser2DDataSet, Decorator<Poser2DDataSet>
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

	public TagsOwner<PoserTag> TagsLibrary { get; }
	public AssetsOwner<Poser2DAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
	public Poser2DDataSet Inner { get; }

	public DomainPoser2DDataSet(Poser2DDataSet inner)
	{
		Inner = inner;
		TagsLibrary = new DomainTagsLibrary<PoserTag>(inner.TagsLibrary);
		AssetsLibrary = new DomainAssetsLibrary<Poser2DAsset>(inner.AssetsLibrary);
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}
}