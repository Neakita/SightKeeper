using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    void LoadWeights<TAsset>(WeightsLibrary<TAsset> library) where TAsset : Asset;
    
    Weights CreateWeights<TAsset>(
        WeightsLibrary<TAsset> library,
        byte[] data,
        DateTime trainedDate,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<TAsset> assets)
        where TAsset : Asset;
}