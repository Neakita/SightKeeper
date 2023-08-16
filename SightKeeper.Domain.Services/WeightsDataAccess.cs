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
        int batch,
        float averageLoss,
        float? accuracy,
        IEnumerable<Asset> assets);

    PreTrainedWeights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        DateTime addedDate);
}