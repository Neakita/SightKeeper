using MemoryPack;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="ItemsAsset{TItem}"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableItemsAsset<TItem> : PackableAsset
{
	public required IReadOnlyCollection<TItem> Items { get; init; }
}