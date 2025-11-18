using CommunityToolkit.Diagnostics;
using MemoryPack;
using MemoryPack.Internal;
using SightKeeper.Data.Services;

namespace SightKeeper.Data;

internal static class Extensions
{
	public static async ValueTask SerializeAsync<T>(
		this Serializer<T> serializer,
		Stream stream,
		T value,
		CancellationToken cancellationToken = default)
	{
		var writer = ReusableLinkedArrayBufferWriterPool.Rent();
		try
		{
			using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
			var memoryPackWriter = new MemoryPackWriter<ReusableLinkedArrayBufferWriter>(ref writer, state);
			serializer.Serialize(ref memoryPackWriter, value);
			await writer.WriteToAndResetAsync(stream, cancellationToken);
			await stream.FlushAsync(cancellationToken);
		}
		finally
		{
			ReusableLinkedArrayBufferWriterPool.Return(writer);
		}
	}
	
	public static async ValueTask<T> DeserializeAsync<T>(
		this Deserializer<T> deserializer,
		Stream stream,
		CancellationToken cancellationToken = default)
	{
		var memoryStream = new MemoryStream();
		await stream.CopyToAsync(memoryStream, cancellationToken);
		var bytes = memoryStream.ToArray();
		using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var reader = new MemoryPackReader(bytes, state);
		return deserializer.Deserialize(ref reader);
	}

	public static string ReadNotNullString(this ref MemoryPackReader reader)
	{
		var value = reader.ReadString();
		Guard.IsNotNull(value);
		return value;
	}
}