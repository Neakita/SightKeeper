using System.Buffers;
using MemoryPack;

namespace SightKeeper.Data;

public interface MemoryPackSerializable
{
	void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer) where TBufferWriter : IBufferWriter<byte>;
}