using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Domain.Services;

public interface AssetsDataAccess
{
    Task LoadItemsAsync(Asset asset, CancellationToken cancellationToken = default);
    void LoadAssets(DataSet dataSet);
    Task LoadAssetsAsync(DataSet dataSet, CancellationToken cancellationToken = default);
}