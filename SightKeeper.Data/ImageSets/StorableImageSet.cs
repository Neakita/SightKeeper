using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

public interface StorableImageSet : ImageSet, IDisposable
{
	new IReadOnlyList<StorableImage> Images { get; }
	new StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size);
	new IReadOnlyList<StorableImage> GetImagesRange(int index, int count);
	StorableImage WrapAndInsertImage(StorableImage image);

	IReadOnlyList<ManagedImage> ImageSet.Images => Images;

	ManagedImage ImageSet.CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return CreateImage(creationTimestamp, size);
	}

	IReadOnlyList<ManagedImage> ImageSet.GetImagesRange(int index, int count)
	{
		return GetImagesRange(index, count);
	}
}