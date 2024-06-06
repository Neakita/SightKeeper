using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Services;

public abstract class WeightsDataAccess : IDisposable
{
	public IObservable<(WeightsLibrary library, Weights weights)> WeightsCreated => _weightsCreated.AsObservable();
	public IObservable<Weights> WeightsRemoved => _weightsRemoved.AsObservable();

	public abstract WeightsData LoadWeightsONNXData(Weights weights);
	public abstract WeightsData LoadWeightsPTData(Weights weights);

	public WeightsDataAccess(ObjectsLookupper objectsLookupper)
	{
		_objectsLookupper = objectsLookupper;
	}

	public Weights CreateWeights(
		WeightsLibrary library,
		byte[] onnxData,
		byte[] ptData,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<ItemClass> itemClasses)
	{
		var weights = library.CreateWeights(modelSize, metrics, itemClasses);
		SaveWeightsData(weights, new WeightsData(onnxData), new WeightsData(ptData));
		_weightsCreated.OnNext((library, weights));
		return weights;
	}
	public void RemoveWeights(Weights weights)
	{
		var library = _objectsLookupper.GetLibrary(weights);
		library.RemoveWeights(weights);
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

	private readonly ObjectsLookupper _objectsLookupper;
	private readonly Subject<(WeightsLibrary, Weights)> _weightsCreated = new();
	private readonly Subject<Weights> _weightsRemoved = new();
}