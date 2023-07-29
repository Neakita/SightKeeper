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
    
    public void Annotate(DetectorModel model, Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox)
    {
        _dbContext.Attach(model);
        var asset = model.MakeAsset(screenshot);
        Annotate(asset, itemClass, boundingBox);
    }

    public void Annotate(DetectorAsset asset, ItemClass itemClass, BoundingBox boundingBox)
    {
        _dbContext.Attach(asset);
        asset.CreateItem(itemClass, boundingBox);
        _dbContext.SaveChanges();
    }

    public void MakeAsset(DetectorModel model, Screenshot screenshot)
    {
        _dbContext.Attach(model);
        model.MakeAsset(screenshot);
        _dbContext.SaveChanges();
    }

    public void Move(DetectorItem item, BoundingBox boundingBox)
    {
        _dbContext.Attach(item);
        item.BoundingBox.SetFromTwoPositions(boundingBox.X1, boundingBox.Y1, boundingBox.X2, boundingBox.Y2);
        _dbContext.SaveChanges();
    }

    public void ChangeItemClass(DetectorItem item, ItemClass newItemClass)
    {
        _dbContext.Attach(item);
        item.ItemClass = newItemClass;
        _dbContext.SaveChanges();
    }

    public void DeleteItem(DetectorAsset asset, DetectorItem item)
    {
        _dbContext.Attach(asset);
        asset.DeleteItem(item);
        _dbContext.SaveChanges();
    }

    public void ReturnToScreenshots(DetectorModel model, DetectorAsset asset)
    {
        _dbContext.Attach(model);
        var screenshot = asset.Screenshot;
        model.DeleteAsset(asset);
        model.ScreenshotsLibrary.AddScreenshot(screenshot);
        _dbContext.SaveChanges();
    }

    public void ClearItems(DetectorAsset asset)
    {
        _dbContext.Attach(asset);
        asset.ClearItems();
        _dbContext.SaveChanges();
    }
    
    private readonly AppDbContext _dbContext;
}