using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotImageLoader : ScreenshotImageLoader
{
    public DbScreenshotImageLoader(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Load(Screenshot screenshot) => _dbContext.Entry(screenshot).Reference(s => s.Image).Load();

    private readonly AppDbContext _dbContext;
}