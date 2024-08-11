using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary;

public sealed class DataSetsDataAccess : ReadDataAccess<DataSet>, ObservableDataAccess<DataSet>, WriteDataAccess<DataSet>
{
	public IReadOnlyCollection<DataSet> Items => _appDataAccess.Data.DataSets;
	public IObservable<DataSet> Added => _added.AsObservable();
	public IObservable<DataSet> Removed => _removed.AsObservable();

	public DataSetsDataAccess(AppDataAccess appDataAccess)
	{
		_appDataAccess = appDataAccess;
	}

	public void Add(DataSet dataSet)
	{
		Guard.IsTrue(_appDataAccess.Data.DataSets.Add(dataSet));
		_added.OnNext(dataSet);
	}

	public void Remove(DataSet dataSet)
	{
		Guard.IsTrue(_appDataAccess.Data.DataSets.Remove(dataSet));
		_removed.OnNext(dataSet);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Subject<DataSet> _added = new();
	private readonly Subject<DataSet> _removed = new();
}