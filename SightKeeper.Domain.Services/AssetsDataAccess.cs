using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface AssetsDataAccess
{
    Task LoadItems(Asset asset, CancellationToken cancellationToken = default);
    Task LoadAssets(DataSet dataSet, CancellationToken cancellationToken = default);
}