using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class PopulateImageLookupperImageSet(ImageSet inner, ImageLookupperPopulator populator) : ImageSet, Decorator<ImageSet>, IDisposable
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

	public IReadOnlyList<ManagedImage> Images => inner.Images;
	public ImageSet Inner => inner;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var image = inner.CreateImage(creationTimestamp, size);
		populator.AddImage(image);
		return image;
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		populator.RemoveImage(image);
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (int i = index; i < index + count; i++)
		{
			var image = Images[i];
			populator.RemoveImage(image);
		}
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		foreach (var image in Images)
			populator.RemoveImage(image);
	}
}