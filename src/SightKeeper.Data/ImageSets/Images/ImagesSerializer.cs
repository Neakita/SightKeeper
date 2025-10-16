using System.Buffers;
using MemoryPack;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class ImagesSerializer : Serializer<IReadOnlyCollection<ManagedImage>>
{
	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, IReadOnlyCollection<ManagedImage> images)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(images.Count);
		foreach (var image in images)
			writer.WriteUnmanaged(image.Id, image.CreationTimestamp, image.Size);
	}
}