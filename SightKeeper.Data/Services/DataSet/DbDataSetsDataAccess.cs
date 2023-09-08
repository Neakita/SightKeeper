using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetsDataAccess : DataSetsDataAccess
{
    public IObservable<Domain.Model.DataSet> DataSetRemoved => _dataSetRemoved.AsObservable();
    
    public DbDataSetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IReadOnlyCollection<Domain.Model.DataSet>> GetDataSets(CancellationToken cancellationToken) =>
        Task.Run(GetDataSets, cancellationToken);

    private IReadOnlyCollection<Domain.Model.DataSet> GetDataSets() =>
        _dbContext.DataSets
            .Include(dataSet => dataSet.ItemClasses)
            .Include(dataSet => dataSet.Game)
            .Include(dataSet => dataSet.ScreenshotsLibrary)
            .ToList();

    public async Task RemoveDataSet(Domain.Model.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.DataSets.Remove(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetRemoved.OnNext(dataSet);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _dataSetRemoved = new();
}