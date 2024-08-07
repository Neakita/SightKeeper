using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal sealed partial class SerializablePoser2DAsset : SerializableAsset
{
	public ImmutableArray<SerializablePoser2DItem> Items { get; }

	public SerializablePoser2DAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<SerializablePoser2DItem> items) : base(screenshotId, usage)
	{
		Items = items;
	}
}