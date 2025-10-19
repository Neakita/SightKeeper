using System.Buffers;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ImageSerializer : Serializer<ManagedImage>
{
	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ManagedImage image)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteUnmanaged(image.Id, image.CreationTimestamp, image.Size);
	}
}