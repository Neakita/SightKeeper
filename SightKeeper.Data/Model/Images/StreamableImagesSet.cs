using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class StreamableImagesSet(ImageSet inner, FileSystemDataAccess dataAccess) : ImageSet
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

	public IReadOnlyList<Image> Images => inner.Images.Select(AsStreamable).ToList();

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var packableImage = inner.CreateImage(creationTimestamp, size);
		return AsStreamable(packableImage);
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count).Select(AsStreamable).ToList();
	}

	public int IndexOf(Image image)
	{
		return inner.IndexOf(image);
	}

	public void RemoveImageAt(int index)
	{
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		inner.RemoveImagesRange(index, count);
	}

	private StreamableDataImage AsStreamable(Image image)
	{
		return new StreamableDataImage((PackableImage)image, dataAccess);
	}
}