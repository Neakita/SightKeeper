using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface AssetItem
{
	DomainTag Tag { get; }
	Bounding Bounding { get; set; }
}