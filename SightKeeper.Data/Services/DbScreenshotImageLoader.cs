using CommunityToolkit.Diagnostics;
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
    
    public ScreenshotImage Load(Screenshot screenshot)
    {
        var entry = _dbContext.Entry(screenshot);
        if (entry.State != EntityState.Detached)
            entry.Reference(s => s.Image).Load();
        Guard.IsNotNull(screenshot.Image);
        return screenshot.Image;
    }

    private readonly AppDbContext _dbContext;
}