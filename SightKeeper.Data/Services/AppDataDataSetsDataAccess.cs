using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Services;

// C-C-Combo!
public sealed class AppDataDataSetsDataAccess :
	ReadDataAccess<DataSet>,
	ObservableDataAccess<DataSet>,
	WriteDataAccess<DataSet>,
	IDisposable
{
	public IReadOnlyCollection<DataSet> Items => _appDataAccess.Data.DataSets;
	public IObservable<DataSet> Added => _added.AsObservable();
	public IObservable<DataSet> Removed => _removed.AsObservable();

	public AppDataDataSetsDataAccess(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public void Add(DataSet dataSet)
	{
		lock (_appDataLock)
			_appDataAccess.Data.AddDataSet(dataSet);
		_appDataAccess.SetDataChanged();
		_added.OnNext(dataSet);
	}

	public void Remove(DataSet dataSet)
	{
		lock (_appDataLock)
			_appDataAccess.Data.RemoveDataSet(dataSet);
		_appDataAccess.SetDataChanged();
		_removed.OnNext(dataSet);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
	private readonly Subject<DataSet> _added = new();
	private readonly Subject<DataSet> _removed = new();
}