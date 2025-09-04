using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class ObservableWeightsLibrary(WeightsLibrary inner) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => _weights;

	public Domain.DataSets.Weights.Weights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var weights = inner.CreateWeights(metadata, tags);
		var change = new Addition<Domain.DataSets.Weights.Weights>
		{
			Items = [weights]
		};
		_weights.Notify(change);
		return weights;
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		inner.RemoveWeights(weights);
		var change = new Removal<Domain.DataSets.Weights.Weights>
		{
			Items = [weights]
		};
		_weights.Notify(change);
	}

	private readonly ExternalObservableCollection<Domain.DataSets.Weights.Weights> _weights = new(inner.Weights);
}