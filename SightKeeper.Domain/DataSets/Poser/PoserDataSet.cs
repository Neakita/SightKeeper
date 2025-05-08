using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser;

public abstract class PoserDataSet : DataSet
{
	public sealed override TagsLibrary<PoserTag> TagsLibrary { get; } = new(PoserTagsFactory.Instance);
	public abstract override PoserWeightsLibrary WeightsLibrary { get; }
}