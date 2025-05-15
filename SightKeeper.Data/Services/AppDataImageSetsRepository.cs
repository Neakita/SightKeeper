using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetsRepository :
	ReadRepository<ImageSet>,
	ObservableRepository<ImageSet>,
	WriteRepository<ImageSet>,
	IDisposable
{
	public IReadOnlyCollection<ImageSet> Items => _appDataAccess.Data.ImageSets;
	public IObservable<ImageSet> Added => _added.AsObservable();
	public IObservable<ImageSet> Removed => _removed.AsObservable();

	public AppDataImageSetsRepository(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public void Add(ImageSet library)
	{
		lock (_appDataLock)
			_appDataAccess.Data.AddImageSet(library);
		_appDataAccess.SetDataChanged();
		_added.OnNext(library);
	}

	public void Remove(ImageSet library)
	{
		lock (_appDataLock)
			_appDataAccess.Data.RemoveImageSet(library);
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