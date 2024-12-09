using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataScreenshotsLibrariesDataAccess :
	ReadDataAccess<ScreenshotsLibrary>,
	ObservableDataAccess<ScreenshotsLibrary>,
	WriteDataAccess<ScreenshotsLibrary>,
	IDisposable
{
	public IReadOnlyCollection<ScreenshotsLibrary> Items => _appDataAccess.Data.ScreenshotsLibraries;
	public IObservable<ScreenshotsLibrary> Added => _added.AsObservable();
	public IObservable<ScreenshotsLibrary> Removed => _removed.AsObservable();

	public AppDataScreenshotsLibrariesDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
	}

	public void Add(ScreenshotsLibrary library)
	{
		lock (_editingLock)
			_appDataAccess.Data.AddScreenshotsLibrary(library);
		_appDataAccess.SetDataChanged();
		_added.OnNext(library);
	}

	public void Remove(ScreenshotsLibrary library)
	{
		lock (_editingLock)
			_appDataAccess.Data.RemoveScreenshotsLibrary(library);
		_appDataAccess.SetDataChanged();
		_removed.OnNext(library);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly Subject<ScreenshotsLibrary> _added = new();
	private readonly Subject<ScreenshotsLibrary> _removed = new();
}