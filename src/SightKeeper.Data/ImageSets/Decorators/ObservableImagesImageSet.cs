using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using Vibrance.Changes;

namespace SightKeeper.Data.ImageSets.Decorators;

// could be named ObservableImageSet,
// but I want to specify it handles only Images observability
internal sealed class ObservableImagesImageSet(ImageSet inner) : ImageSet, Decorator<ImageSet>, IDisposable
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public IReadOnlyList<ManagedImage> Images => _images;
	public ImageSet Inner => inner;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var index = Images.Count;
		var image = inner.CreateImage(creationTimestamp, size);
		if (_images.HasObservers)
		{
			Insertion<ManagedImage> change = new()
			{
				Index = index,
				Items = [image]
			};
			_images.Notify(change);
		}
		return image;
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		inner.RemoveImageAt(index);
		if (_images.HasObservers)
		{
			IndexedRemoval<ManagedImage> change = new()
			{
				Index = index,
				Items = [image]
			};
			_images.Notify(change);
		}
	}

	public void RemoveImagesRange(int index, int count)
	{
		var images = GetImagesRange(index, count);
		inner.RemoveImagesRange(index, count);
		if (_images.HasObservers)
		{
			IndexedRemoval<ManagedImage> change = new()
			{
				Index = index,
				Items = images
			};
			_images.Notify(change);
		}
	}

	public void Dispose()
	{
		_images.Dispose();
	}

	private readonly ExternalObservableList<ManagedImage> _images = new(inner.Images);
}