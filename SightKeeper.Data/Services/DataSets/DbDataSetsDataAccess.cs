using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.DataSets;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Services.DataSets;

public sealed class DbDataSetsDataAccess : DataSetsDataAccess
{
    public IObservable<DetectorDataSet> DataSetAdded => _dataSetAdded.AsObservable();
    public IObservable<DetectorDataSet> DataSetRemoved => _dataSetRemoved.AsObservable();
    public IEnumerable<DetectorDataSet> DataSets => _dataSets;
    
    public DbDataSetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dataSets = new HashSet<DetectorDataSet>(_dbContext.DataSets);
    }

    public void AddDataSet(DetectorDataSet dataSet)
    {
	    bool isAdded = _dataSets.Add(dataSet);
	    Guard.IsTrue(isAdded);
        _dbContext.Add(dataSet);
        _dataSetAdded.OnNext(dataSet);
    }

    public void RemoveDataSet(DetectorDataSet dataSet)
    {
	    var isRemoved = _dataSets.Remove(dataSet);
	    Guard.IsTrue(isRemoved);
        _dbContext.DataSets.Remove(dataSet);
        _dataSetRemoved.OnNext(dataSet);
    }

    private readonly AppDbContext _dbContext;
    private readonly HashSet<DetectorDataSet> _dataSets;
    private readonly Subject<DetectorDataSet> _dataSetAdded = new();
    private readonly Subject<DetectorDataSet> _dataSetRemoved = new();
}