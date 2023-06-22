using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface ModelWeightsDataAccess
{
    void AddWeights(Model.Abstract.Model model, ModelWeights weights);
}