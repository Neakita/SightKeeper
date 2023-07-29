using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
    public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Load(ScreenshotsLibrary library)
    {
        _dbContext.Entry(library).Collection(lib => lib.Screenshots).Load();
    }

    public void SaveChanges(ScreenshotsLibrary library)
    {
        _dbContext.Update(library);
        _dbContext.SaveChanges();
    }
    
    private readonly AppDbContext _dbContext;
}