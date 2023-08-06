using Microsoft.EntityFrameworkCore;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotImageLoader : ScreenshotImageLoader
{
    public DbScreenshotImageLoader(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Load(Screenshot screenshot)
    {
        var entry = _dbContext.Entry(screenshot);
        if (entry.State == EntityState.Detached)
            return;
        entry.Reference(s => s.Image).Load();
    }

    private readonly AppDbContext _dbContext;
}