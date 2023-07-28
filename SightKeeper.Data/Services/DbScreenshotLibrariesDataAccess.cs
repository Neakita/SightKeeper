using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotLibrariesDataAccess : ScreenshotLibrariesDataAccess
{
    public DbScreenshotLibrariesDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void SaveChanges(ScreenshotsLibrary library)
    {
        _dbContext.Update(library);
        _dbContext.SaveChanges();
    }
    
    private readonly AppDbContext _dbContext;
}