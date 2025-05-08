using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface AssetItem
{
	Tag Tag { get; }
	Bounding Bounding { get; set; }
}