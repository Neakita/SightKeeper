using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class DataRemovingWeightsLibrary(WeightsLibrary inner) : WeightsLibrary, Decorator<WeightsLibrary>
{
	public IReadOnlyCollection<WeightsData> Weights => inner.Weights;
	public WeightsLibrary Inner => inner;

	public WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		return inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(WeightsData weights)
	{
		inner.RemoveWeights(weights);
		weights.GetFirst<DeletableData>().DeleteData();
	}
}