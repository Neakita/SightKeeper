using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    IObservable<Weights> WeightsCreated { get; }
    IObservable<Weights> WeightsDeleted { get; }
    Task LoadWeights(WeightsLibrary library, CancellationToken cancellationToken = default);

    Task<Weights> CreateWeights(
        WeightsLibrary library,
        byte[] data,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<Asset> assets,
        CancellationToken cancellationToken = default);

    Task DeleteWeights(Weights weights, CancellationToken cancellationToken);
}