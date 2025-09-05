using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class ObservableWeightsLibrary(WeightsLibrary inner) : WeightsLibrary
{
	public IReadOnlyCollection<WeightsData> Weights => _weights;

	public WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var weights = inner.CreateWeights(metadata, tags);
		var change = new Addition<WeightsData>
		{
			Items = [weights]
		};
		_weights.Notify(change);
		return weights;
	}

	public void RemoveWeights(WeightsData weights)
	{
		inner.RemoveWeights(weights);
		var change = new Removal<WeightsData>
		{
			Items = [weights]
		};
		_weights.Notify(change);
	}

	private readonly ExternalObservableCollection<WeightsData> _weights = new(inner.Weights);
}