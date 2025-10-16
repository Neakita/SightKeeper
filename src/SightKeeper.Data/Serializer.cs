using System.Buffers;
using MemoryPack;

namespace SightKeeper.Data;

internal interface Serializer<in T>
{
	void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, T value)
		where TBufferWriter : IBufferWriter<byte>;
}