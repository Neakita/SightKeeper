using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal sealed partial class Poser2DAsset : Asset
{
	public ImmutableArray<Poser2DItem> Items { get; }

	public Poser2DAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<Poser2DItem> items) : base(screenshotId, usage)
	{
		Items = items;
	}
}