using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetsRepository :
	ReadRepository<ImageSet>,
	ObservableRepository<ImageSet>,
	WriteRepository<ImageSet>,
	ReadRepository<StorableImageSet>,
	ObservableRepository<StorableImageSet>,
	WriteRepository<StorableImageSet>,
	IDisposable
{
	IReadOnlyCollection<ImageSet> ReadRepository<ImageSet>.Items => _appDataAccess.Data.ImageSets;
	IObservable<ImageSet> ObservableRepository<ImageSet>.Added => _added;
	IObservable<ImageSet> ObservableRepository<ImageSet>.Removed => _removed;

	public IReadOnlyCollection<StorableImageSet> Items => _appDataAccess.Data.ImageSets;
	public IObservable<StorableImageSet> Added => _added;
	public IObservable<StorableImageSet> Removed => _removed;

	public void Add(StorableImageSet set)
	{
		lock (_appDataLock)
			_appDataAccess.Data.AddImageSet(set);
		_changeListener.SetDataChanged();
		_added.OnNext(set);
	}

	public void Remove(StorableImageSet set)
	{
		bool canDelete = set.CanDelete();
		Guard.IsTrue(canDelete);
		lock (_appDataLock)
		{
			_appDataAccess.Data.RemoveImageSet(set);
			DeleteImagesData(set);
		}
		_changeListener.SetDataChanged();
		_removed.OnNext(set);
	}

	void WriteRepository<ImageSet>.Add(ImageSet set)
	{
		Add((StorableImageSet)set);
	}

	void WriteRepository<ImageSet>.Remove(ImageSet set)
	{
		Remove((StorableImageSet)set);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	internal AppDataImageSetsRepository(Lock appDataLock, AppDataAccess appDataAccess, ChangeListener changeListener)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
		_changeListener = changeListener;
	}

	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;
	private readonly ChangeListener _changeListener;
	private readonly Subject<StorableImageSet> _added = new();
	private readonly Subject<StorableImageSet> _removed = new();

	private void DeleteImagesData(ImageSet set)
	{
		foreach (var image in set.Images)
		{
			var streamableDataImage = image.UnWrapDecorator<StreamableDataImage>();
			streamableDataImage.DeleteData();
		}
	}
}