using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public interface DetectorItem : AssetItem
{
	new Tag Tag { get; set; }
	Tag AssetItem.Tag => Tag;
}