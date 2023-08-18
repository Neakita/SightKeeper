using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    void LoadWeights(WeightsLibrary library);
    
    Weights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<Asset> assets);
}