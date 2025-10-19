using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class TrackableWeightsLibrary(WeightsLibrary inner, ChangeListener listener) : WeightsLibrary, Decorator<WeightsLibrary>
{
	public IReadOnlyCollection<WeightsData> Weights => inner.Weights;
	public WeightsLibrary Inner => inner;

	public WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var weights = inner.CreateWeights(metadata, tags);
		listener.SetDataChanged();
		return weights;
	}

	public void RemoveWeights(WeightsData weights)
	{
		inner.RemoveWeights(weights);
		listener.SetDataChanged();
	}
}