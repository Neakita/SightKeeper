using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Services;

public abstract class DetectorWeightsDataAccess : IDisposable
{
	public IObservable<DetectorWeights> WeightsCreated => _weightsCreated.AsObservable();
	public IObservable<DetectorWeights> WeightsRemoved => _weightsRemoved.AsObservable();

	public abstract WeightsData LoadWeightsONNXData(DetectorWeights weights);
	public abstract WeightsData LoadWeightsPTData(DetectorWeights weights);

	public DetectorWeights CreateWeights(
		DetectorWeightsLibrary library,
		byte[] onnxData,
		byte[] ptData,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<DetectorTag> tags)
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(onnxData), new WeightsData(ptData));
		_weightsCreated.OnNext(weights);
		return weights;
	}

	public void RemoveWeights(DetectorWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
		_weightsRemoved.OnNext(weights);
	}

	public void Dispose()
	{
		_weightsCreated.Dispose();
		_weightsRemoved.Dispose();
		GC.SuppressFinalize(this);
	}

	protected abstract void SaveWeightsData(DetectorWeights weights, WeightsData onnxData, WeightsData ptData);
	protected abstract void RemoveWeightsData(DetectorWeights weights);

	private readonly Subject<DetectorWeights> _weightsCreated = new();
	private readonly Subject<DetectorWeights> _weightsRemoved = new();
}