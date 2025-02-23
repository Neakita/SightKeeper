using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser;

public abstract class PoserDataSet : DataSet
{
	public abstract override TagsLibrary<PoserTag> TagsLibrary { get; }
	public abstract override PoserWeightsLibrary WeightsLibrary { get; }
}