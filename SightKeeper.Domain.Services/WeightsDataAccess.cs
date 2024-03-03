using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    IObservable<Weights> WeightsCreated { get; }
    IObservable<Weights> WeightsDeleted { get; }
    void LoadWeights(WeightsLibrary weightsLibrary);
    Task LoadWeightsAsync(WeightsLibrary weightsLibrary, CancellationToken cancellationToken = default);

    Task<Weights> CreateWeights(
        WeightsLibrary weightsLibrary,
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        WeightsMetrics weightsMetrics,
        IEnumerable<ItemClass> itemClasses,
        CancellationToken cancellationToken = default);

    Task DeleteWeights(Weights weights, CancellationToken cancellationToken);

    Task<WeightsData> LoadWeightsData(Weights weights, WeightsFormat weightsFormat, CancellationToken cancellationToken = default);
}