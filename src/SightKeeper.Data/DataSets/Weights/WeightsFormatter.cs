using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class WeightsFormatter
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
				weights.Metadata.Resolution);
			writer.WriteString(weights.Metadata.Model);
		}
	}

	public void ReadWeights(ref MemoryPackReader reader, WeightsLibrary library)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var weightsCount));
		var innermostLibrary = library.GetFirst<InMemoryWeightsLibrary>();
		for (int i = 0; i < weightsCount; i++)
		{
			reader.ReadUnmanaged(
				out Id id,
				out DateTimeOffset creationTimestamp,
				out Vector2<ushort> resolution);
			var model = reader.ReadString();
			Guard.IsNotNull(model);
			var weightsMetadata = new WeightsMetadata
			{
				Model = model,
				CreationTimestamp = creationTimestamp,
				Resolution = resolution
			};
			var weights = new InMemoryWeights(id, weightsMetadata);
			innermostLibrary.AddWeights(weights);
		}
	}
}