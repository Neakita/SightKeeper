using FlakeId;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ImageDeserializer : Deserializer<ManagedImage>
{
	public ManagedImage Deserialize(ref MemoryPackReader reader)
	{
		reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
		return new InMemoryImage(id, creationTimestamp, size);
	}
}