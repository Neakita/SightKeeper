using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    void LoadWeights(WeightsLibrary library);
    
    InternalTrainedWeights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        int epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<Asset> assets);

    PreTrainedWeights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        DateTime addedDate);
}