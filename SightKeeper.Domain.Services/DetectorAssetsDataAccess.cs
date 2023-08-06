using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Services;

public interface DetectorAssetsDataAccess
{
    void LoadItems(DetectorAsset asset);
}