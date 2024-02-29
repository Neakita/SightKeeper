using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.DataSet.Weights;

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
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<ItemClass> itemClasses,
        CancellationToken cancellationToken = default);

    Task DeleteWeights(Weights weights, CancellationToken cancellationToken);

    Task<WeightsData> LoadWeightsData(Weights weights, WeightsFormat format, CancellationToken cancellationToken = default);
}