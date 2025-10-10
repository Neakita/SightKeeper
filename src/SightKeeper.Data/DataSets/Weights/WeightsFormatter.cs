using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal static class WeightsFormatter
{
	public static void WriteWeights<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<WeightsData> weightsCollection,
		IReadOnlyDictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(weightsCollection.Count);
		foreach (var weights in weightsCollection)
		{
			var weightsId = weights.Id;
			writer.WriteUnmanaged(
				weightsId,
				weights.Metadata.Model,
				weights.Metadata.CreationTimestamp,
				weights.Metadata.ModelSize,
				weights.Metadata.Metrics,
				weights.Metadata.Resolution);
			using var memoryOwner = MemoryPool<byte>.Shared.Rent(weights.Tags.Count);
			var weightsTagIndexes = memoryOwner.Memory.Span[..weights.Tags.Count];
			for (int i = 0; i < weights.Tags.Count; i++)
			{
				var tag = weights.Tags[i];
				weightsTagIndexes[i] = tagIndexes[tag];
			}
			writer.WriteUnmanagedSpan(weightsTagIndexes);
		}
	}

	public static void ReadWeights(ref MemoryPackReader reader, WeightsLibrary library, IReadOnlyList<Tag> tags)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var weightsCount));
		var innermostLibrary = library.Get<InMemoryWeightsLibrary>();
		for (int i = 0; i < weightsCount; i++)
		{
			reader.ReadUnmanaged(
				out Id id,
				out Model model,
				out DateTimeOffset creationTimestamp,
				out ModelSize modelSize,
				out WeightsMetrics metrics,
				out Vector2<ushort> resolution);
			Span<byte> tagIndexes = new();
			reader.ReadUnmanagedSpan(ref tagIndexes);
			List<Tag> weightsTags = new(tagIndexes.Length);
			foreach (var tagIndex in tagIndexes)
			{
				var tag = tags[tagIndex];
				weightsTags.Add(tag);
			}
			var weightsMetadata = new WeightsMetadata
			{
				Model = model,
				CreationTimestamp = creationTimestamp,
				ModelSize = modelSize,
				Metrics = metrics,
				Resolution = resolution
			};
			var weights = new InMemoryWeights(id, weightsMetadata, weightsTags);
			innermostLibrary.AddWeights(weights);
		}
	}
}