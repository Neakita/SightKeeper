using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services.Annotating;

public sealed class DbDetectorAnnotator : DetectorAnnotator
{
    public DbDetectorAnnotator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<DetectorItem> Annotate(Screenshot screenshot, ItemClass itemClass, Bounding bounding, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(bounding.Left, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Right, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Top, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Bottom, 0, 1);
        var asset = screenshot.Asset ??
                    screenshot.Library.DataSet.MakeAsset(screenshot);
        var item = asset.CreateItem(itemClass, bounding);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task MarkAsset(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        Guard.IsNull(screenshot.Asset);
        var screenshotDataSet = screenshot.Library.DataSet;
        await _dbContext.Entry(screenshotDataSet).Collection(dataSet => dataSet.Assets).LoadAsync(cancellationToken: cancellationToken);
        screenshotDataSet.MakeAsset(screenshot);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UnMarkAsset(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        DeleteAsset(screenshot);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteScreenshot(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        if (screenshot.Asset != null)
            DeleteAsset(screenshot);
        screenshot.Library.DeleteScreenshot(screenshot);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteItem(DetectorItem item, CancellationToken cancellationToken = default)
    {
        item.Asset.DeleteItem(item);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task ChangeItemClass(DetectorItem item, ItemClass itemClass, CancellationToken cancellationToken = default)
    {
        item.ItemClass = itemClass;
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Move(DetectorItem item, Bounding bounding, CancellationToken cancellationToken = default)
    {
        item.Bounding.SetFromBounding(bounding);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;

    private static void DeleteAsset(Screenshot screenshot)
    {
        Guard.IsNotNull(screenshot.Asset);
        var dataSet = screenshot.Library.DataSet;
        dataSet.DeleteAsset(screenshot.Asset);
    }
}