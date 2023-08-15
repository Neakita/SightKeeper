using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    void LoadWeights(ModelWeightsLibrary library);
    
    InternalTrainedModelWeights CreateWeights(
        ModelWeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        int batch,
        float averageLoss,
        float? accuracy,
        IEnumerable<Asset> assets);

    PreTrainedModelWeights CreateWeights(
        ModelWeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        DateTime addedDate);
}