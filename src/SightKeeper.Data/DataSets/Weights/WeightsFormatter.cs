using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class WeightsFormatter(TagIndexProvider tagIndexProvider)
{
	public void WriteWeights<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<WeightsData> weightsCollection)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(weightsCollection.Count);
		foreach (var weights in weightsCollection)
		{
			var weightsId = weights.Id;
			writer.WriteUnmanaged(
				weightsId,
				weights.Metadata.CreationTimestamp,
				weights.Metadata.Metrics,
				weights.Metadata.Resolution);
			writer.WriteString(weights.Metadata.Model);
			using var memoryOwner = MemoryPool<byte>.Shared.Rent(weights.Tags.Count);
			var weightsTagIndexes = memoryOwner.Memory.Span[..weights.Tags.Count];
			for (int i = 0; i < weights.Tags.Count; i++)
			{
				var tag = weights.Tags[i];
				weightsTagIndexes[i] = tagIndexProvider.GetTagIndex(tag);
			}
			writer.WriteUnmanagedSpan(weightsTagIndexes);
		}
	}

	public void ReadWeights(ref MemoryPackReader reader, WeightsLibrary library, IReadOnlyList<Tag> tags)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var weightsCount));
		var innermostLibrary = library.GetFirst<InMemoryWeightsLibrary>();
		for (int i = 0; i < weightsCount; i++)
		{
			reader.ReadUnmanaged(
				out Id id,
				out DateTimeOffset creationTimestamp,
				out WeightsMetrics metrics,
				out Vector2<ushort> resolution);
			var model = reader.ReadString();
			Guard.IsNotNull(model);
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
				Metrics = metrics,
				Resolution = resolution
			};
			var weights = new InMemoryWeights(id, weightsMetadata, weightsTags);
			innermostLibrary.AddWeights(weights);
		}
	}
}