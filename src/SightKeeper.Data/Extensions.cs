using MemoryPack;
using MemoryPack.Internal;
using SightKeeper.Data.Services;
using SightKeeper.Domain;

namespace SightKeeper.Data;

internal static class Extensions
{
	public static T GetInnermost<T>(this object obj)
	{
		return obj.GetSelfAndChildren().OfType<T>().Last();
	}

	public static T Get<T>(this object obj)
	{
		return obj.GetSelfAndChildren().OfType<T>().First();
	}

	private static IEnumerable<object> GetSelfAndChildren(this object obj)
	{
		yield return obj;
		while (obj is Decorator<object> decorator)
		{
			obj = decorator.Inner;
			yield return obj;
		}
	}
	
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
	
	public static async ValueTask<T?> DeserializeAsync<T>(
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
}