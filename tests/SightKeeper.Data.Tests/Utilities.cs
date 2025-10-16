using System.Buffers;
using MemoryPack;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	public static byte[] Serialize<T>(this Serializer<T> serializer, T value)
	{
		ArrayBufferWriter<byte> bufferWriter = new();
		using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var writer = new MemoryPackWriter<ArrayBufferWriter<byte>>(ref bufferWriter, state);
		serializer.Serialize(ref writer, value);
		writer.Flush();
		return bufferWriter.WrittenSpan.ToArray();
	}

	public static T Deserialize<T>(this Deserializer<T> deserializer, byte[] bytes)
	{
		using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var reader = new MemoryPackReader(bytes, state);
		return deserializer.Deserialize(ref reader);
	}
}