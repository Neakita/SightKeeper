using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Data.Images;
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

	public void Add(ImageSet set)
	{
		lock (_appDataLock)
			_appDataAccess.Data.AddImageSet(set);
		_changeListener.SetDataChanged();
		_added.OnNext(set);
	}

	public void Remove(ImageSet set)
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
	private readonly Subject<ImageSet> _added = new();
	private readonly Subject<ImageSet> _removed = new();

	private void DeleteImagesData(ImageSet set)
	{
		foreach (var image in set.Images)
		{
			var streamableDataImage = image.UnWrapDecorator<StreamableDataImage>();
			streamableDataImage.DeleteData();
		}
	}
}