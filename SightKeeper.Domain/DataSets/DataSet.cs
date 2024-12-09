using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public abstract class DataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public abstract TagsLibrary TagsLibrary { get; }
	public abstract AssetsLibrary AssetsLibrary { get; }
	public abstract WeightsLibrary WeightsLibrary { get; }
}