using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface DetectorAssetsDataAccess
{
    void LoadItems(Asset asset);
}