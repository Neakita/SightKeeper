using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	public static T PersistUsingFormatter<T>(this T value, IMemoryPackFormatter<T> formatter)
	{
		var buffer = SerializeUsingFormatter(value, formatter);
		var persistedValue = DeserializeUsingFormatter(buffer, formatter);
		Guard.IsNotNull(persistedValue);
		return persistedValue;
	}

	public static byte[] SerializeUsingFormatter<T>(this T? value, IMemoryPackFormatter<T> formatter)
	{
		ArrayBufferWriter<byte> bufferWriter = new();
		using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var writer = new MemoryPackWriter<ArrayBufferWriter<byte>>(ref bufferWriter, state);
		formatter.Serialize(ref writer, ref value);
		writer.Flush();
		return bufferWriter.WrittenSpan.ToArray();
	}

	public static T? DeserializeUsingFormatter<T>(ReadOnlySpan<byte> buffer, IMemoryPackFormatter<T> formatter)
	{
		using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var reader = new MemoryPackReader(buffer, state);
		var value = default(T);
		formatter.Deserialize(ref reader, ref value);
		return value;
	}
}