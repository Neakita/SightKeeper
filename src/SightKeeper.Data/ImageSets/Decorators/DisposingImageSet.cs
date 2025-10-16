using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class DisposingImageSet(ImageSet inner) : ImageSet, Decorator<ImageSet>, IDisposable
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

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return inner.CreateImage(creationTimestamp, size);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		image.Dispose();
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (int i = index; i < index + count; i++)
		{
			var image = Images[i];
			image.Dispose();
		}
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		foreach (var image in Images)
			image.Dispose();
	}
}