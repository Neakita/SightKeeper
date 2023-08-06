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
    public DbDetectorAnnotator(AppDbContext dbContext, ItemClassDataAccess itemClassDataAccess)
    {
        _dbContext = dbContext;
        _itemClassDataAccess = itemClassDataAccess;
    }
    
    public DetectorItem Annotate(Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox)
    {
        Guard.IsBetweenOrEqualTo(boundingBox.X1, 0, 1);
        Guard.IsBetweenOrEqualTo(boundingBox.X2, 0, 1);
        Guard.IsBetweenOrEqualTo(boundingBox.Y1, 0, 1);
        Guard.IsBetweenOrEqualTo(boundingBox.Y2, 0, 1);
        var asset = screenshot.GetOptionalAsset<DetectorAsset>() ??
                    screenshot.Library.GetModel<DetectorModel>().MakeAsset(screenshot);
        _itemClassDataAccess.LoadItems(itemClass);
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

    public void Move(DetectorItem item, BoundingBox bounding)
    {
        item.BoundingBox.SetFromBounding(bounding);
        _dbContext.SaveChanges();
    }

    private readonly AppDbContext _dbContext;
    private readonly ItemClassDataAccess _itemClassDataAccess;

    private static void DeleteAsset(Screenshot screenshot)
    {
        Guard.IsNotNull(screenshot.Asset);
        var model = screenshot.Library.GetModel<DetectorModel>();
        model.DeleteAsset(screenshot.GetAsset<DetectorAsset>());
    }
}