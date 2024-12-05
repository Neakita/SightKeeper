using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="ItemsAsset{TItem}"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableItemsAsset<TItem> : PackableAsset
{
	public required ImmutableArray<TItem> Items { get; init; }
}