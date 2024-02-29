using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet.Screenshots;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
    public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        await _dbContext.Entry(library).Collection(lib => lib.Screenshots).LoadAsync(cancellationToken);
        return library.Screenshots;
    }

    public bool IsLoaded(ScreenshotsLibrary library)
    {
        return _dbContext.Entry(library).Collection(lib => lib.Screenshots).IsLoaded;
    }

    public void CreateScreenshot(ScreenshotsLibrary library, byte[] content)
    {
        var screenshot = library.CreateScreenshot(content);
        _dbContext.Attach(screenshot);
    }

    public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(library);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}