using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal static class WeightsFormatter
{
	public static void WriteWeights<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<WeightsData> weightsCollection)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(weightsCollection.Count);
		foreach (var weights in weightsCollection)
			WriteWeights(ref writer, weights);
	}

	public static void ReadWeights(ref MemoryPackReader reader, WeightsLibrary library)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var weightsCount));
		var innermostLibrary = library.GetFirst<InMemoryWeightsLibrary>();
		for (int i = 0; i < weightsCount; i++)
		{
			var weights = ReadWeights(ref reader);
			innermostLibrary.AddWeights(weights);
		}
	}

	private static void WriteWeights<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, WeightsData weights)
		where TBufferWriter : IBufferWriter<byte>
	{
		var weightsId = GetWeightsId(weights);
		writer.WriteUnmanaged(weightsId);
		WriteMetadata(ref writer, weights.Metadata);
	}

	private static InMemoryWeights ReadWeights(ref MemoryPackReader reader)
	{
		var id = reader.ReadUnmanaged<Id>();
		var weightsMetadata = ReadMetadata(ref reader);
		var weights = new InMemoryWeights(id, weightsMetadata);
		return weights;
	}

	private static void WriteMetadata<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		WeightsMetadata metadata)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteUnmanaged(
			metadata.CreationTimestamp,
			metadata.Resolution);
		writer.WriteString(metadata.Model);
		writer.WriteString(metadata.Format);
	}

	private static WeightsMetadata ReadMetadata(ref MemoryPackReader reader)
	{
		reader.ReadUnmanaged(
			out DateTimeOffset creationTimestamp,
			out Vector2<ushort> resolution);
		var model = reader.ReadNotNullString();
		var format = reader.ReadNotNullString();
		return new WeightsMetadata
		{
			Model = model,
			Format = format,
			CreationTimestamp = creationTimestamp,
			Resolution = resolution
		};
	}

	private static Id GetWeightsId(WeightsData weights)
	{
		var idHolder = weights.GetFirst<IdHolder>();
		return idHolder.Id;
	}
}