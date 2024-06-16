using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Services;

public abstract class ClassifierWeightsDataAccess : IDisposable
{
	public IObservable<ClassifierWeights> WeightsCreated => _weightsCreated.AsObservable();
	public IObservable<ClassifierWeights> WeightsRemoved => _weightsRemoved.AsObservable();

	public abstract WeightsData LoadWeightsONNXData(ClassifierWeights weights);
	public abstract WeightsData LoadWeightsPTData(ClassifierWeights weights);

	public ClassifierWeights CreateWeights(
		ClassifierWeightsLibrary library,
		byte[] onnxData,
		byte[] ptData,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<ClassifierTag> tags)
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(onnxData), new WeightsData(ptData));
		_weightsCreated.OnNext(weights);
		return weights;
	}

	public void RemoveWeights(ClassifierWeights weights)
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

	protected abstract void SaveWeightsData(ClassifierWeights weights, WeightsData onnxData, WeightsData ptData);
	protected abstract void RemoveWeightsData(ClassifierWeights weights);

	private readonly Subject<ClassifierWeights> _weightsCreated = new();
	private readonly Subject<ClassifierWeights> _weightsRemoved = new();
}