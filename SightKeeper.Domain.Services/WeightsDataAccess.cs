using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    IObservable<Weights> WeightsCreated { get; }
    IObservable<Weights> WeightsDeleted { get; }
    void LoadWeights(WeightsLibrary library);
    Task LoadWeightsAsync(WeightsLibrary library, CancellationToken cancellationToken = default);

    Task<Weights> CreateWeights(
        WeightsLibrary library,
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        WeightsMetrics metrics,
        IEnumerable<ItemClass> itemClasses,
        CancellationToken cancellationToken = default);

    Task DeleteWeights(Weights weights, CancellationToken cancellationToken);

    Task<WeightsData> LoadWeightsData(Weights weights, WeightsFormat format, CancellationToken cancellationToken = default);
}