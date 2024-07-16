using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal sealed partial class SerializablePoserAsset : SerializableAsset
{
	public ImmutableArray<SerializablePoserItem> Items { get; }

	public SerializablePoserAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<SerializablePoserItem> items) : base(screenshotId, usage)
	{
		Items = items;
	}
}