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
    
    public void Annotate(Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox)
    {
        var asset = screenshot.GetOptionalAsset<DetectorAsset>() ??
                    screenshot.Library.GetModel<DetectorModel>().MakeAsset(screenshot);
        asset.CreateItem(itemClass, boundingBox);
        _dbContext.SaveChanges();
    }

    private readonly AppDbContext _dbContext;
}