using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotImageLoader : ScreenshotImageLoader
{
    public DbScreenshotImageLoader(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Image Load(Screenshot screenshot)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (screenshot.Image != null)
            return screenshot.Image;
        lock (_dbContext)
        {
            var entry = _dbContext.Entry(screenshot);
            entry.Reference(s => s.Image).Load();
            Guard.IsNotNull(screenshot.Image);
            return screenshot.Image;
        }
    }

    public Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (screenshot.Image != null)
            return Task.FromResult(screenshot.Image);
        return Task.Run(() =>
        {
            lock (_dbContext)
                return _dbContext.Entry(screenshot).Reference(s => s.Image).Query().AsNoTracking().Single();
        }, cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}