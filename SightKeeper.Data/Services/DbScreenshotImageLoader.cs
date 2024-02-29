using Microsoft.EntityFrameworkCore;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotImageLoader : ScreenshotImageLoader
{
    public DbScreenshotImageLoader(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        if (screenshot.Image != null)
            return Task.FromResult(screenshot.Image);
        return _dbContext.Entry(screenshot).Reference(s => s.Image).Query().AsNoTracking().SingleAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}