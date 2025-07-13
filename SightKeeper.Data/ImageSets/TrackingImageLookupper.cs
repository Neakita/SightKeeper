using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Data.ImageSets.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.ImageSets;

internal sealed class TrackingImageLookupper : ImageLookupper, IDisposable
{
	public TrackingImageLookupper(ReadRepository<StorableImageSet> imageSetReadRepository, ObservableRepository<StorableImageSet> imageSetObservableRepository)
	{
		foreach (var set in imageSetReadRepository.Items)
			AddImageSet(set);
		imageSetObservableRepository.Added.Subscribe(AddImageSet).DisposeWith(_constructorDisposable);
		imageSetObservableRepository.Removed.Subscribe(RemoveImageSet).DisposeWith(_constructorDisposable);
	}

	public StorableImage GetImage(Id id)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		return _images[id];
	}

	public void Dispose()
	{
		if (_isDisposed)
			return;
		_constructorDisposable.Dispose();
		foreach (var disposable in _subscriptions.Values)
			disposable.Dispose();
		_isDisposed = true;
	}

	private readonly CompositeDisposable _constructorDisposable = new();
	private readonly Dictionary<StorableImageSet, IDisposable> _subscriptions = new();
	private readonly Dictionary<Id, StorableImage> _images = new();
	private bool _isDisposed;

	private void AddImageSet(StorableImageSet set)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		var subscription = ((ReadOnlyObservableList<StorableImage>)set.Images).Subscribe(OnImagesChange);
		_subscriptions.Add(set, subscription);
	}

	private void RemoveImageSet(StorableImageSet set)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		RemoveImages(set.Images);
		var isRemoved = _subscriptions.Remove(set, out var subscription);
		Guard.IsTrue(isRemoved);
		subscription!.Dispose();
	}

	private void OnImagesChange(Change<StorableImage> change)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		RemoveImages(change.OldItems);
		AddImages(change.NewItems);
	}

	private void RemoveImages(IEnumerable<StorableImage> images)
	{
		foreach (var image in images)
		{
			var id = image.Id;
			bool isRemoved = _images.Remove(id);
			Guard.IsTrue(isRemoved);
		}
	}

	private void AddImages(IEnumerable<StorableImage> images)
	{
		foreach (var image in images)
		{
			var id = image.Id;
			_images.Add(id, image);
		}
	}
}