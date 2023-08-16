using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.Model;

public sealed class DbDataSetsDataAccess : DataSetsDataAccess
{
    public IObservable<Domain.Model.DataSet> DataSetRemoved => _dataSetRemoved.AsObservable();
    
    public DbDataSetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Domain.Model.DataSet>> GetDataSets(CancellationToken cancellationToken = default) =>
        await _dbContext.Models
            .Include(model => model.ItemClasses)
            .Include(model => model.Game)
            .Include(model => model.ScreenshotsLibrary)
            .ToListAsync(cancellationToken);

    public async Task RemoveDataSet(Domain.Model.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.Models.Remove(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetRemoved.OnNext(dataSet);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _dataSetRemoved = new();
}