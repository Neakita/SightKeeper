using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class ObservableWeightsLibrary(StorableWeightsLibrary inner) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => _weights;

	public StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var weights = inner.CreateWeights(metadata, tags);
		var change = new Addition<StorableWeights>
		{
			Items = [weights]
		};
		_weights.Notify(change);
		return weights;
	}

	public void RemoveWeights(StorableWeights weights)
	{
		inner.RemoveWeights(weights);
		var change = new Removal<StorableWeights>
		{
			Items = [weights]
		};
		_weights.Notify(change);
	}

	private readonly ExternalObservableCollection<StorableWeights> _weights = new(inner.Weights);
}