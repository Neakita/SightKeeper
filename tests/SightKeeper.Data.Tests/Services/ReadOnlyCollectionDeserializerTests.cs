using System.Buffers;
using FluentAssertions;
using MemoryPack;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests.Services;

public sealed class ReadOnlyCollectionDeserializerTests
{
	[Fact]
	public void ShouldCallDeserializer()
	{
		var itemDeserializer = new SubstituteDeserializer<int>(() => 0);
		var deserializer = new ReadOnlyCollectionDeserializer<int>(itemDeserializer);
		var bytes = GetCollectionBytes([1, 2, 3]);
		deserializer.Deserialize(bytes);
		itemDeserializer.CallsCounts.Should().Be(3);
	}

	private static byte[] GetCollectionBytes(IReadOnlyCollection<int> items)
	{
		ArrayBufferWriter<byte> bufferWriter = new();
		using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var writer = new MemoryPackWriter<ArrayBufferWriter<byte>>(ref bufferWriter, state);
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
			writer.WriteUnmanaged(item);
		writer.Flush();
		return bufferWriter.WrittenSpan.ToArray();
	}
}