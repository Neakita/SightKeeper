using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class ObservableWeightsLibrary(WeightsLibrary inner) : WeightsLibrary, Decorator<WeightsLibrary>, IDisposable
{
	public IReadOnlyCollection<WeightsData> Weights => _weights;
	public WeightsLibrary Inner => inner;

	public WeightsData CreateWeights(WeightsMetadata metadata)
	{
		var weights = inner.CreateWeights(metadata);
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

	public void Dispose()
	{
		_weights.Dispose();
	}

	private readonly ExternalObservableCollection<WeightsData> _weights = new(inner.Weights);
}