using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface DetectorItem : BoundedItem, TagUser
{
	Tag Tag { get; set; }
}