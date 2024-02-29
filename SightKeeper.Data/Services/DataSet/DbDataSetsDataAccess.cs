using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetsDataAccess : DataSetsDataAccess
{
    public IObservable<Domain.Model.DataSets.DataSet> DataSetAdded => _dataSetAdded.AsObservable();
    public IObservable<Domain.Model.DataSets.DataSet> DataSetUpdated => _dataSetUpdated.AsObservable();
    public IObservable<Domain.Model.DataSets.DataSet> DataSetRemoved => _dataSetRemoved.AsObservable();
    
    public DbDataSetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Domain.Model.DataSets.DataSet>> GetDataSets(CancellationToken cancellationToken)
    {
        return _dbContext.DataSets
            .Include(dataSet => dataSet.ItemClasses.OrderBy(itemClass => itemClass.Id))
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsNameFree(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.DataSets.AllAsync(dataSet => dataSet.Name != name, cancellationToken);
    }

    public async Task AddDataSet(Domain.Model.DataSets.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(dataSet, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateDataSet(Domain.Model.DataSets.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(dataSet);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveDataSet(Domain.Model.DataSets.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        _dbContext.DataSets.Remove(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetRemoved.OnNext(dataSet);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSets.DataSet> _dataSetAdded = new();
    private readonly Subject<Domain.Model.DataSets.DataSet> _dataSetUpdated = new();
    private readonly Subject<Domain.Model.DataSets.DataSet> _dataSetRemoved = new();
}