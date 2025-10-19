using System.Buffers;
using MemoryPack;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests;

internal sealed class SubstituteSerializer<T> : Serializer<T>
{
	public List<T> Calls { get; } = new();

	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, T value) where TBufferWriter : IBufferWriter<byte>
	{
		Calls.Add(value);
	}
}