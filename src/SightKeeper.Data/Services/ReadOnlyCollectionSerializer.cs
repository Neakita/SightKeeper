using System.Buffers;
using MemoryPack;

namespace SightKeeper.Data.Services;

internal sealed class ReadOnlyCollectionSerializer<T>(Serializer<T> itemSerializer) : Serializer<IReadOnlyCollection<T>>
{
	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, IReadOnlyCollection<T> items)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
			itemSerializer.Serialize(ref writer, item);
	}
}