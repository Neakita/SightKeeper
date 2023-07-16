using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbModelsDataAccess : ModelsDataAccess
{
    public DbModelsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<Model>> GetModels(CancellationToken cancellationToken = default) =>
        await _dbContext.Models
            .Include(model => model.ItemClasses)
            .Include(model => model.Game)
            .Include(model => model.Config)
            .Include(model => model.ScreenshotsLibrary)
            .ToListAsync(cancellationToken);

    public Task RemoveModel(Model model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.Models.Remove(model);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}