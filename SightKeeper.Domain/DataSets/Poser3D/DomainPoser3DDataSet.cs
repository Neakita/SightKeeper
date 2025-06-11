using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class DomainPoser3DDataSet : Poser3DDataSet, Decorator<Poser3DDataSet>
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
	public DomainAssetsLibrary<Poser3DAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
	public Poser3DDataSet Inner { get; }

	public DomainPoser3DDataSet(Poser3DDataSet inner)
	{
		Inner = inner;
		TagsLibrary = new DomainTagsLibrary<PoserTag>(inner.TagsLibrary);
		AssetsLibrary = new DomainAssetsLibrary<Poser3DAsset>(inner.AssetsLibrary);
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary);
	}
}