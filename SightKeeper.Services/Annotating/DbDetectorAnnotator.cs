using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Model.Extensions;

namespace SightKeeper.Services.Annotating;

public sealed class DbDetectorAnnotator : DetectorAnnotator
{
    public DbDetectorAnnotator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public DetectorItem Annotate(Screenshot screenshot, ItemClass itemClass, Bounding bounding)
    {
        Guard.IsBetweenOrEqualTo(bounding.Left, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Right, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Top, 0, 1);
        Guard.IsBetweenOrEqualTo(bounding.Bottom, 0, 1);
        var asset = screenshot.GetOptionalAsset<DetectorAsset>() ??
                    screenshot.Library.GetDataSet<DetectorAsset>().MakeAsset(screenshot);
        var item = asset.CreateItem(itemClass, bounding);
        _dbContext.SaveChanges();
        return item;
    }

    public void MarkAsset(Screenshot screenshot)
    {
        Guard.IsNull(screenshot.Asset);
        var screenshotDataSet = screenshot.Library.GetDataSet<DetectorAsset>();
        _dbContext.Entry(screenshotDataSet).Collection(dataSet => dataSet.Assets).Load();
        screenshotDataSet.MakeAsset(screenshot);
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

    public void Move(DetectorItem item, Bounding bounding)
    {
        item.Bounding.SetFromBounding(bounding);
        _dbContext.SaveChanges();
    }

    private readonly AppDbContext _dbContext;

    private static void DeleteAsset(Screenshot screenshot)
    {
        Guard.IsNotNull(screenshot.Asset);
        var model = screenshot.Library.GetDataSet<DetectorAsset>();
        model.DeleteAsset(screenshot.GetAsset<DetectorAsset>());
    }
}