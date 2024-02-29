using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services.Annotating;

public sealed class DbDetectorAnnotator : DetectorAnnotator
{
    public DbDetectorAnnotator(AppDbContext dbContext, ItemClassDataAccess itemClassDataAccess, AssetsDataAccess assetsDataAccess)
    {
        _dbContext = dbContext;
        _itemClassDataAccess = itemClassDataAccess;
        _assetsDataAccess = assetsDataAccess;
    }

    public DetectorItem Annotate(Screenshot screenshot, ItemClass itemClass, Bounding bounding)
    {
	    Guard.IsBetweenOrEqualTo(bounding.Left, 0, 1);
	    Guard.IsBetweenOrEqualTo(bounding.Right, 0, 1);
	    Guard.IsBetweenOrEqualTo(bounding.Top, 0, 1);
	    Guard.IsBetweenOrEqualTo(bounding.Bottom, 0, 1);
	    _assetsDataAccess.LoadAssets(screenshot.Library.DataSet);
	    var asset = screenshot.Asset ??
	                screenshot.Library.DataSet.MakeAsset(screenshot);
	    _itemClassDataAccess.LoadItems(itemClass);
	    var item = asset.CreateItem(itemClass, bounding);
	    _dbContext.SaveChanges();
	    return item;
    }

    public async Task<DetectorItem> AnnotateAsync(Screenshot screenshot, ItemClass itemClass, Bounding bounding, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(bounding.Left, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Right, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Top, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Bottom, 0, 1);
        await _assetsDataAccess.LoadAssetsAsync(screenshot.Library.DataSet, cancellationToken);
        var asset = screenshot.Asset ??
                    screenshot.Library.DataSet.MakeAsset(screenshot);
        await _itemClassDataAccess.LoadItemsAsync(itemClass, cancellationToken);
        var item = asset.CreateItem(itemClass, bounding);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task MarkAssetAsync(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        Guard.IsNull(screenshot.Asset);
        var screenshotDataSet = screenshot.Library.DataSet;
        await _dbContext.Entry(screenshotDataSet).Collection(dataSet => dataSet.Assets).LoadAsync(cancellationToken: cancellationToken);
        screenshotDataSet.MakeAsset(screenshot);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UnMarkAssetAsync(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        DeleteAsset(screenshot);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteScreenshotAsync(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        if (screenshot.Asset != null)
            DeleteAsset(screenshot);
        screenshot.Library.DeleteScreenshot(screenshot);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteItemAsync(DetectorItem item, CancellationToken cancellationToken = default)
    {
        item.Asset.DeleteItem(item);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void ChangeItemClass(DetectorItem item, ItemClass itemClass)
    {
	    item.ItemClass = itemClass;
	    _dbContext.SaveChanges();
    }

    public Task ChangeItemClassAsync(DetectorItem item, ItemClass itemClass, CancellationToken cancellationToken = default)
    {
        item.ItemClass = itemClass;
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Move(DetectorItem item, Bounding bounding)
    {
	    item.Bounding.SetFromBounding(bounding);
	    _dbContext.SaveChanges();
    }

    public Task MoveAsync(DetectorItem item, Bounding bounding, CancellationToken cancellationToken = default)
    {
        item.Bounding.SetFromBounding(bounding);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
    private readonly ItemClassDataAccess _itemClassDataAccess;
    private readonly AssetsDataAccess _assetsDataAccess;

    private static void DeleteAsset(Screenshot screenshot)
    {
        Guard.IsNotNull(screenshot.Asset);
        var dataSet = screenshot.Library.DataSet;
        dataSet.DeleteAsset(screenshot.Asset);
    }
}