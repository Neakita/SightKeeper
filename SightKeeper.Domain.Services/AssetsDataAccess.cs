using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets;

namespace SightKeeper.Domain.Services;

public interface AssetsDataAccess
{
    Task LoadItemsAsync(Asset asset, CancellationToken cancellationToken = default);
    void LoadAssets(DataSet dataSet);
    Task LoadAssetsAsync(DataSet dataSet, CancellationToken cancellationToken = default);
}