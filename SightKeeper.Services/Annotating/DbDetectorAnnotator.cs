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
    
    public DetectorItem Annotate(Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox)
    {
        var asset = screenshot.GetOptionalAsset<DetectorAsset>() ??
                    screenshot.Library.GetModel<DetectorModel>().MakeAsset(screenshot);
        var item = asset.CreateItem(itemClass, boundingBox);
        _dbContext.SaveChanges();
        return item;
    }

    public void MarkAsset(Screenshot screenshot)
    {
        Guard.IsNull(screenshot.Asset);
        var model = screenshot.Library.GetModel<DetectorModel>();
        model.MakeAsset(screenshot);
        _dbContext.SaveChanges();
    }

    public void UnMarkAsset(Screenshot screenshot)
    {
        DeleteAsset(screenshot);
        _dbContext.SaveChanges();
    }

    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (screenshot.Asset != null)
            DeleteAsset(screenshot);
        screenshot.Library.DeleteScreenshot(screenshot);
        _dbContext.SaveChanges();
    }

    public void DeleteItem(DetectorItem item)
    {
        item.Asset.DeleteItem(item);
        _dbContext.SaveChanges();
    }

    public void ChangeItemClass(DetectorItem item, ItemClass itemClass)
    {
        item.ItemClass = itemClass;
        _dbContext.SaveChanges();
    }

    private readonly AppDbContext _dbContext;

    private static void DeleteAsset(Screenshot screenshot)
    {
        Guard.IsNotNull(screenshot.Asset);
        var model = screenshot.Library.GetModel<DetectorModel>();
        model.DeleteAsset(screenshot.GetAsset<DetectorAsset>());
    }
}