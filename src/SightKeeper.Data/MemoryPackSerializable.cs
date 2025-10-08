using System.Buffers;
using MemoryPack;

namespace SightKeeper.Data;

internal interface MemoryPackSerializable
{
	void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer) where TBufferWriter : IBufferWriter<byte>;
}