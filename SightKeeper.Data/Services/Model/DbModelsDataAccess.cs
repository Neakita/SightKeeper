using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.Model;

public sealed class DbModelsDataAccess : ModelsDataAccess
{
    public IObservable<Domain.Model.Model> ModelRemoved => _modelRemoved.AsObservable();
    
    public DbModelsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Domain.Model.Model>> GetModels(CancellationToken cancellationToken = default) =>
        await _dbContext.Models
            .Include(model => model.ItemClasses)
            .Include(model => model.Game)
            .Include(model => model.ScreenshotsLibrary)
            .ToListAsync(cancellationToken);

    public Task RemoveModel(Domain.Model.Model model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.Models.Remove(model);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.Model> _modelRemoved = new();
}