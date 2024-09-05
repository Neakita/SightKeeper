using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="ItemsAsset{TItem}"/>
/// </summary>
internal sealed class PackableItemsAsset<TItem> : PackableAsset
{
	public ImmutableArray<TItem> Items { get; }

	public PackableItemsAsset(
		AssetUsage usage, 
		uint screenshotId, 
		ImmutableArray<TItem> items)
		: base(usage, screenshotId)
	{
		Items = items;
	}
}