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
	void RemoveImage(Image image);
	void RemoveImageAt(int index);
	void RemoveImagesRange(int index, int count);
}