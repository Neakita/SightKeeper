namespace SightKeeper.Domain.Images;

public interface ImageSet
{
	string Name { get; set; }
	string Description { get; set; }

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	IReadOnlyList<Image> Images { get; }

	Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size);
	IReadOnlyList<Image> GetImagesRange(int index, int count);
	int IndexOf(Image image);

	void RemoveImageAt(int index);
	void RemoveImagesRange(int index, int count);

	void RemoveImage(Image image)
	{
		var index = IndexOf(image);
		if (index < 0)
			throw new ArgumentException("Image not found", nameof(image));
	}
}