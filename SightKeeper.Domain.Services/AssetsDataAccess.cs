using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface AssetsDataAccess
{
    Task LoadItemsAsync(Asset asset, CancellationToken cancellationToken = default);
    void LoadAssets(DataSet dataSet);
    Task LoadAssetsAsync(DataSet dataSet, CancellationToken cancellationToken = default);
}