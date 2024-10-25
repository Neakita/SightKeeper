using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Services;

public sealed class DataSetsDataAccess : ReadDataAccess<DataSet>, ObservableDataAccess<DataSet>,
	WriteDataAccess<DataSet>
{
	public IReadOnlyCollection<DataSet> Items => _appDataAccess.Data.DataSets;
	public IObservable<DataSet> Added => _added.AsObservable();
	public IObservable<DataSet> Removed => _removed.AsObservable();

	public DataSetsDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public void Add(DataSet dataSet)
	{
		bool isAdded;
		lock (_editingLock)
			isAdded = _appDataAccess.Data.DataSets.Add(dataSet);
		Guard.IsTrue(isAdded);
		_appDataAccess.SetDataChanged();
		_added.OnNext(dataSet);
	}

	public void Remove(DataSet dataSet)
	{
		bool isRemoved;
		lock (_editingLock)
			isRemoved = _appDataAccess.Data.DataSets.Remove(dataSet);
		_screenshotsDataAccess.DeleteAllScreenshotsData(dataSet.ScreenshotsLibrary);
		Guard.IsTrue(isRemoved);
		_appDataAccess.SetDataChanged();
		_removed.OnNext(dataSet);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly Subject<DataSet> _added = new();
	private readonly Subject<DataSet> _removed = new();
}