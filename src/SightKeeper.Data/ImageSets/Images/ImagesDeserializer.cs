using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ImagesDeserializer : Deserializer<IReadOnlyCollection<ManagedImage>>
{
	public IReadOnlyCollection<ManagedImage> Deserialize(ref MemoryPackReader reader)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var imagesCount));
		var imagesArray = new ManagedImage[imagesCount];
		for (int i = 0; i < imagesCount; i++)
		{
			var image = ReadImage(ref reader);
			imagesArray[i] = image;
		}
		return imagesArray;
	}

	private static InMemoryImage ReadImage(ref MemoryPackReader reader)
	{
		reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
		return new InMemoryImage(id, creationTimestamp, size);
	}
}