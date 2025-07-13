using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal interface StorableImageSet : ImageSet
{
	new IReadOnlyList<StorableImage> Images { get; }
	new StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size);
	new IReadOnlyList<StorableImage> GetImagesRange(int index, int count);

	IReadOnlyList<Image> ImageSet.Images => Images;

	Image ImageSet.CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return CreateImage(creationTimestamp, size);
	}

	IReadOnlyList<Image> ImageSet.GetImagesRange(int index, int count)
	{
		return GetImagesRange(index, count);
	}
}