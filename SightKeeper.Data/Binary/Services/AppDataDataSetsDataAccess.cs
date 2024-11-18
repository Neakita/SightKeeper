using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Services;

// C-C-Combo!
public sealed class AppDataDataSetsDataAccess :
	ReadDataAccess<DataSet>,
	ObservableDataAccess<DataSet>,
	WriteDataAccess<DataSet>
{
	public IReadOnlyCollection<DataSet> Items => _appDataAccess.Data.DataSets;
	public IObservable<DataSet> Added => _added.AsObservable();
	public IObservable<DataSet> Removed => _removed.AsObservable();

	public AppDataDataSetsDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public void Add(DataSet dataSet)
	{
		lock (_editingLock)
			_appDataAccess.Data.AddDataSet(dataSet);
		_appDataAccess.SetDataChanged();
		_added.OnNext(dataSet);
	}

	public void Remove(DataSet dataSet)
	{
		lock (_editingLock)
			_appDataAccess.Data.RemoveDataSet(dataSet);
		_screenshotsDataAccess.DeleteAllScreenshotsData(dataSet.ScreenshotsLibrary);
		_appDataAccess.SetDataChanged();
		_removed.OnNext(dataSet);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly Subject<DataSet> _added = new();
	private readonly Subject<DataSet> _removed = new();
}