using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
internal partial class SerializableDetectorAsset : SerializableAsset
{
	public ImmutableArray<SerializableDetectorItem> Items { get; }

	public SerializableDetectorAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<SerializableDetectorItem> items) : base(screenshotId, usage)
	{
		Items = items;
	}
}