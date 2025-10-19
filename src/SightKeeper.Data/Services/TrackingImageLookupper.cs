using FlakeId;
using SightKeeper.Application;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Services;

internal sealed class TrackingImageLookupper : ImageLookupper, IDisposable
{
	public TrackingImageLookupper(
		ObservableListRepository<ImageSet> imageSetsRepository)
	{
		_disposable = imageSetsRepository.Items
			.TransformMany(set => (ReadOnlyObservableList<ManagedImage>)set.Images)
			.Subscribe(HandleImagesChange);
	}

	public ManagedImage GetImage(Id id)
	{
		return _images[id];
	}

	public bool ContainsImage(Id id)
	{
		return _images.ContainsKey(id);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly IDisposable _disposable;
	private readonly Dictionary<Id, ManagedImage> _images = new();

	private void HandleImagesChange(IndexedChange<ManagedImage> change)
	{
		foreach (var oldImage in change.OldItems)
			_images.Remove(oldImage.Id);
		foreach (var newImage in change.NewItems)
			_images.Add(newImage.Id, newImage);
	}
}