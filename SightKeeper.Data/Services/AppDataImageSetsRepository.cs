using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetsRepository :
	ReadRepository<ImageSet>,
	ObservableRepository<ImageSet>,
	WriteRepository<ImageSet>,
	IDisposable
{
	[Tag(typeof(AppData))] public required Lock AppDataLock { get; init; }
	public required AppDataAccess AppDataAccess { get; init; }
	public required ChangeListener ChangeListener { get; init; }
	public required WriteImageDataAccess ImageDataAccess { get; init; }

	public IReadOnlyCollection<ImageSet> Items => AppDataAccess.Data.ImageSets;
	public IObservable<ImageSet> Added => _added.AsObservable();
	public IObservable<ImageSet> Removed => _removed.AsObservable();

	public void Add(ImageSet set)
	{
		lock (AppDataLock)
			AppDataAccess.Data.AddImageSet(set);
		ChangeListener.SetDataChanged();
		_added.OnNext(set);
	}

	public void Remove(ImageSet set)
	{
		bool canDelete = set.CanDelete();
		Guard.IsTrue(canDelete);
		lock (AppDataLock)
		{
			AppDataAccess.Data.RemoveImageSet(set);
			DeleteImagesData(set);
		}
		ChangeListener.SetDataChanged();
		_removed.OnNext(set);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	private readonly Subject<ImageSet> _added = new();
	private readonly Subject<ImageSet> _removed = new();

	private void DeleteImagesData(ImageSet set)
	{
		foreach (var image in set.Images)
			ImageDataAccess.DeleteImageData(image);
	}
}