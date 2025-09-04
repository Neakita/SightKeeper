namespace SightKeeper.Domain.Images;

public interface ImageSet : IDisposable
{
	string Name { get; set; }
	string Description { get; set; }

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	IReadOnlyList<ManagedImage> Images { get; }

	ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size);
	IReadOnlyList<ManagedImage> GetImagesRange(int index, int count);

	void RemoveImageAt(int index);
	void RemoveImagesRange(int index, int count);
}