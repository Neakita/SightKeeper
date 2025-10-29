using System.Buffers;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ImageSerializer : Serializer<ManagedImage>
{
	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ManagedImage image)
		where TBufferWriter : IBufferWriter<byte>
	{
		var idHolder = image.GetFirst<IdHolder>();
		var imageId = idHolder.Id;
		writer.WriteUnmanaged(imageId, image.CreationTimestamp, image.Size);
	}
}