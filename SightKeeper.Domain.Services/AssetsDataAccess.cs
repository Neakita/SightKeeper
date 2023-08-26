using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface AssetsDataAccess
{
    void LoadItems(Asset asset);
}