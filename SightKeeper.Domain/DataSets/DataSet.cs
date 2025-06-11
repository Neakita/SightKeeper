using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public interface DataSet
{
	string Name { get; set; }
	string Description { get; set; }

	public TagsOwner<Tag> TagsLibrary { get; }
	AssetsOwner<Asset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
}