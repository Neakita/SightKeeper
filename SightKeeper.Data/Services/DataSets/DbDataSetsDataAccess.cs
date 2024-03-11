using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.DataSets;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Services.DataSets;

public sealed class DbDataSetsDataAccess : DataSetsDataAccess
{
    public IObservable<DataSet> DataSetAdded => _dataSetAdded.AsObservable();
    public IObservable<DataSet> DataSetRemoved => _dataSetRemoved.AsObservable();
    public IEnumerable<DataSet> DataSets => _dataSets;
    
    public DbDataSetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dataSets = new HashSet<DataSet>(_dbContext.DataSets);
    }

    public void AddDataSet(DataSet dataSet)
    {
	    bool isAdded = _dataSets.Add(dataSet);
	    Guard.IsTrue(isAdded);
        _dbContext.Add(dataSet);
        _dataSetAdded.OnNext(dataSet);
    }

    public void RemoveDataSet(DataSet dataSet)
    {
	    var isRemoved = _dataSets.Remove(dataSet);
	    Guard.IsTrue(isRemoved);
        _dbContext.DataSets.Remove(dataSet);
        _dataSetRemoved.OnNext(dataSet);
    }

    private readonly AppDbContext _dbContext;
    private readonly HashSet<DataSet> _dataSets;
    private readonly Subject<DataSet> _dataSetAdded = new();
    private readonly Subject<DataSet> _dataSetRemoved = new();
}