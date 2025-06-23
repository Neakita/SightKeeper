using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	private static readonly Lazy<PersistenceServices> Services = new(PersistenceBootstrapper.Setup);

	public static ImageSet CreateImageSet()
	{
		var factory = Services.Value.ImageSetFactory;
		return factory.CreateImageSet();
	}

	public static ClassifierDataSet CreateClassifierDataSet()
	{
		var factory = Services.Value.ClassifierDataSetFactory;
		return factory.CreateDataSet();
	}

	public static T Persist<T>(this T value)
	{
		PersistenceBootstrapper.Setup();
		var serialized = MemoryPackSerializer.Serialize(value);
		var persistedValue = MemoryPackSerializer.Deserialize<T>(serialized);
		Guard.IsNotNull(persistedValue);
		return persistedValue;
	}

	public static Image CreateImage()
	{
		var set = CreateImageSet();
		return set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}

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