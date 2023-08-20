using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface WeightsDataAccess
{
    void LoadWeights(WeightsLibrary library);

    Weights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        ModelSize size,
        uint epoch,
        float loss);
}