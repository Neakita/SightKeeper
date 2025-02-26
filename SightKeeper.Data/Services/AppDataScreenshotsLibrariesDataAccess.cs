using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataScreenshotsLibrariesDataAccess :
	ReadDataAccess<ImageSet>,
	ObservableDataAccess<ImageSet>,
	WriteDataAccess<ImageSet>,
	IDisposable
{
	public IReadOnlyCollection<ImageSet> Items => _appDataAccess.Data.ScreenshotsLibraries;
	public IObservable<ImageSet> Added => _added.AsObservable();
	public IObservable<ImageSet> Removed => _removed.AsObservable();

	public AppDataScreenshotsLibrariesDataAccess(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public void Add(ImageSet library)
	{
		lock (_appDataLock)
			_appDataAccess.Data.AddScreenshotsLibrary(library);
		_appDataAccess.SetDataChanged();
		_added.OnNext(library);
	}

	public void Remove(ImageSet library)
	{
		lock (_appDataLock)
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
	private readonly Lock _appDataLock;
	private readonly Subject<ImageSet> _added = new();
	private readonly Subject<ImageSet> _removed = new();
}