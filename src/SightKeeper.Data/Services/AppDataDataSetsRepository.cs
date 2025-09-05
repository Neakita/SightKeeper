using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.Services;

public sealed class AppDataDataSetsRepository :
	ReadRepository<DataSet<Asset>>,
	ObservableRepository<DataSet<Asset>>,
	WriteRepository<DataSet<Asset>>,
	IDisposable
{
	public IReadOnlyCollection<DataSet<Asset>> Items => _appDataAccess.Data.DataSets;
	public IObservable<DataSet<Asset>> Added => _added.AsObservable();
	public IObservable<DataSet<Asset>> Removed => _removed.AsObservable();

	public void Add(DataSet<Asset> set)
	{
		lock (_appDataLock)
			_appDataAccess.Data.AddDataSet(set);
		_changeListener.SetDataChanged();
		_added.OnNext(set);
	}

	public void Remove(DataSet<Asset> set)
	{
		lock (_appDataLock)
		{
			_appDataAccess.Data.RemoveDataSet(set);
			set.AssetsLibrary.ClearAssets();
		}
		_changeListener.SetDataChanged();
		_removed.OnNext(set);
		set.AssetsLibrary.ClearAssets();
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	public AppDataDataSetsRepository(AppDataAccess appDataAccess, ChangeListener changeListener, Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_changeListener = changeListener;
		_appDataLock = appDataLock;
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly ChangeListener _changeListener;
	private readonly Lock _appDataLock;
	private readonly Subject<DataSet<Asset>> _added = new();
	private readonly Subject<DataSet<Asset>> _removed = new();
}