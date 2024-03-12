using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class WeightsDataAccess : IDisposable
{
	public IObservable<Weights> WeightsCreated => _weightsCreated.AsObservable();
	public IObservable<Weights> WeightsRemoved => _weightsRemoved.AsObservable();

	public abstract WeightsData LoadWeightsONNXData(Weights weights);
	public abstract WeightsData LoadWeightsPTData(Weights weights);

	public Weights CreateWeights(
		WeightsLibrary library,
		byte[] onnxData,
		byte[] ptData,
		Size size,
		WeightsMetrics metrics,
		IReadOnlyCollection<ItemClass> itemClasses)
	{
		var weights = library.CreateWeights(size, metrics, itemClasses);
		SaveWeightsData(weights, new WeightsData(onnxData), new WeightsData(ptData));
		_weightsCreated.OnNext(weights);
		return weights;
	}
	public void RemoveWeights(Weights weights)
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

	protected abstract void SaveWeightsData(Weights weights, WeightsData onnxData, WeightsData ptData);
	protected abstract void RemoveWeightsData(Weights weights);

	private readonly Subject<Weights> _weightsCreated = new();
	private readonly Subject<Weights> _weightsRemoved = new();
}