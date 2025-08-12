using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface AssetItem : BoundedItem, TagUser
{
	Tag Tag { get; }
}